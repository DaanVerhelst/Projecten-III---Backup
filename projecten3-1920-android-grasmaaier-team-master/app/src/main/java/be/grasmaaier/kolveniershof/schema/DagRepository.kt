package be.grasmaaier.kolveniershof.schema

import androidx.lifecycle.LiveData
import androidx.lifecycle.Transformations
import be.grasmaaier.kolveniershof.database.DatabaseDagAtelierClient
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import be.grasmaaier.kolveniershof.network.OverzichtApi
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.text.SimpleDateFormat
import java.util.*
import kotlin.collections.ArrayList

class DagRepository(private val database: KolveniersHofDatabase) {
    private val dateFormat : SimpleDateFormat = SimpleDateFormat("yyyy-MM-dd")

    suspend fun refreshClients(currentDate: Date, persoonId : Long) {
        val datums = getDatumsInWeek(currentDate)
        withContext(Dispatchers.IO){
            var weekVoorClient = OverzichtApi.retrofitService.getWeekVoorClient(dateFormat.format(datums[0]), persoonId).await()
            var gson = Gson().toJson(convertWeek(weekVoorClient, currentDate))
            database.dagDao.insertAteliers(DatabaseDagAtelierClient(
                datum = dateFormat.format(datums[0]),
                client = persoonId,
                data = gson
            ))
        }
    }

    fun getWeekFromPeroon(persoonId: Long, currentDate: Date) : LiveData<List<DagProperty>>{
        var weekObj = database.dagDao.getAteliers(persoonId, dateFormat.format(getDatumsInWeek(currentDate)[0]))
        val listType = object : TypeToken<ArrayList<DagProperty>>() {}.type
        return Transformations.map(weekObj) {
            if (it == null){
                convertWeek(listOf(), currentDate)
            } else {
                Gson().fromJson<List<DagProperty>>(it.data, listType)
            }
        }
    }


    private fun convertWeek( week : List<DagProperty>, currentDate: Date) : List<DagProperty>{
        var week_bis : ArrayList<DagProperty> = ArrayList()
        for(a in getDatumsInWeek(currentDate).subList(0,5)){
            var check = false
            for(b in week){
                if (a.date == b.dateParsed.date){
                    week_bis.add(b)
                    check = true
                    break
                }
            }
            if (!check){
                val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                week_bis.add(DagProperty(dateFormat.format(a), ArrayList<AtelierPropery>()))
            }
        }
        return week_bis
    }

    private fun getDatumsInWeek(today: Date) : List<Date>{
        var dates : ArrayList<Date> = ArrayList()
        for(x in 1..7){
            val calendar = Calendar.getInstance()
            calendar.add(Calendar.DAY_OF_YEAR, x - today.day)
            dates.add(calendar.time)
        }
        return dates
    }
}