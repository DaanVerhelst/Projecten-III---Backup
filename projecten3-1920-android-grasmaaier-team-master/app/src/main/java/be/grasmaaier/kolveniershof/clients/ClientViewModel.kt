package be.grasmaaier.kolveniershof.clients

import androidx.lifecycle.LiveData
import androidx.lifecycle.ViewModel
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch

class ClientViewModel(val database: KolveniersHofDatabase) : ViewModel() {

    private var viewModelJob = Job()
    private val coroutineScope = CoroutineScope(viewModelJob + Dispatchers.Main)
    private val clientsRepository = ClientRepository(database)

    private val _clienten = clientsRepository.clients
    val clienten: LiveData<List<PersoonProperty>>
        get() = _clienten

    init {
        getClients()
    }

    private fun getClients(){
        coroutineScope.launch {
            clientsRepository.refreshClients()
        }
    }

    override fun onCleared() {
        super.onCleared()
        viewModelJob.cancel()
    }
}