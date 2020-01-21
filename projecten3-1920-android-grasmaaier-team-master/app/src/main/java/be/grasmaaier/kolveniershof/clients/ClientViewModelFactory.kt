package be.grasmaaier.kolveniershof.clients

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import java.lang.IllegalArgumentException

class ClientViewModelFactory(private val database: KolveniersHofDatabase) : ViewModelProvider.Factory {
    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(ClientViewModel::class.java)){
            return ClientViewModel(database) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}