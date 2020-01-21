package be.grasmaaier.kolveniershof.startup

import android.content.SharedPreferences
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import be.grasmaaier.kolveniershof.network.AccountApi
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch
import retrofit2.HttpException

class StartUpViewModel(sharedPreferences: SharedPreferences) : ViewModel() {
    private var sharedPreferences : SharedPreferences

    private val _loggedIn = MutableLiveData<Boolean>()
    val loggedIn: LiveData<Boolean>
        get() = _loggedIn

    private val _errorMessage = MutableLiveData<String>()
    val errorMessage: LiveData<String>
        get() = _errorMessage

    private var _viewModelJob = Job()
    private val _coroutineScope = CoroutineScope(_viewModelJob + Dispatchers.Main)

    init {
        this.sharedPreferences = sharedPreferences
        AccountApi.sharedPreferences = sharedPreferences
    }

    private fun check_logged_in() : Boolean{
        if (sharedPreferences.getString("token", null) != null){
            return true
        }
        return false
    }

    fun getToken(){
        if (check_logged_in()){
            checkToken()
        } else {
            throw Exception("Nog niet ingelogd.")
        }
    }

    private fun checkToken(){
        _coroutineScope.launch {
            try {
                AccountApi.retrofitService.checkToken().await()
                _loggedIn.value = true
            } catch (t: HttpException){
                if (t.code() == 401) {
                    _loggedIn.value = false
                    _errorMessage.value = t.message
                } else {
                    _errorMessage.value = t.message
                }
            } catch (t: Throwable){
                _errorMessage.value = t.message
            }
        }
    }
}