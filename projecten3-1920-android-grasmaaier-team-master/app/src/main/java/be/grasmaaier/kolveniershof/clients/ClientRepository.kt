package be.grasmaaier.kolveniershof.clients

import androidx.lifecycle.LiveData
import androidx.lifecycle.Transformations
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import be.grasmaaier.kolveniershof.database.asDomainModel
import be.grasmaaier.kolveniershof.network.PersonenApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class ClientRepository(private val database: KolveniersHofDatabase) {

    val clients: LiveData<List<PersoonProperty>> = Transformations.map(database.clientsDao.getClients()) {
        it.asDomainModel()
    }

    suspend fun refreshClients() {
        withContext(Dispatchers.IO){
            val clients = PersonenApi.retrofitService.getClienten().await()
            database.clientsDao.insertAll(*clients.asDatabaseModel().toTypedArray())
        }
    }
}