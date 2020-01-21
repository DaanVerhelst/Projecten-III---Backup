package be.grasmaaier.kolveniershof.schema

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import be.grasmaaier.kolveniershof.clients.PersoonProperty
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import java.net.ConnectException
import java.util.*

class DetailViewModel(persoon: PersoonProperty, val database: KolveniersHofDatabase) : ViewModel() {
    val persoon : PersoonProperty = persoon
    private var viewModelJob = Job()
    private val coroutineScope = CoroutineScope(viewModelJob + Dispatchers.Main)
    val week: LiveData<List<DagProperty>>
        get() = _week
    private val dagRepository = DagRepository(database)
    private val _week : LiveData<List<DagProperty>> = dagRepository.getWeekFromPeroon(persoon.id, Date())
    val error: MutableLiveData<String> = MutableLiveData()

    init {
        coroutineScope.launch {
            try {
                dagRepository.refreshClients(Date(), persoon.id)
            } catch (ce: ConnectException){
                error.value = "Er is geen internet verbdinging!"
            }
        }
    }
}