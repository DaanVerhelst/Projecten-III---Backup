package be.grasmaaier.kolveniershof.pictogame

import android.util.Log
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import be.grasmaaier.kolveniershof.network.AtelierApi
import be.grasmaaier.kolveniershof.schema.AtelierPropery
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch

class AtelierViewModel : ViewModel() {
    private val _Ateliers = MutableLiveData<List<AtelierPropery>>()
    val ateliers: LiveData<List<AtelierPropery>>
        get() = _Ateliers

    private var viewModelJob = Job()
    private val coroutineScope = CoroutineScope(viewModelJob + Dispatchers.Main)

    init {
        getAteliers();
    }

    private fun getAteliers(){
        coroutineScope.launch {
            try {
                var getAteliersDeffered = AtelierApi.retrofitService.getAteliers()
                _Ateliers.value = getAteliersDeffered.await()
            } catch (t: Throwable){
                Log.e("AtelierViewModel", "getAteliers, onFailure called.")
            }
        }
    }
}