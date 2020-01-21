package be.grasmaaier.kolveniershof.schema

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import be.grasmaaier.kolveniershof.clients.PersoonProperty
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase

class DetailViewModelFactory(private  val persoon : PersoonProperty, private val database: KolveniersHofDatabase) : ViewModelProvider.Factory {
    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(DetailViewModel::class.java)){
            return DetailViewModel(persoon, database) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}