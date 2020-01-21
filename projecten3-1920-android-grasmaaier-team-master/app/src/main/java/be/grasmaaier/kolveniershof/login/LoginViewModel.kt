package be.grasmaaier.kolveniershof.login

import android.content.SharedPreferences
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import be.grasmaaier.kolveniershof.network.AccountApi
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.launch

class LoginViewModel(sharedPreferences: SharedPreferences) : ViewModel() {
    var sharedPreferences : SharedPreferences

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
    fun login(email: String, password : String){
        _coroutineScope.launch {
            try {

                val getJWTDeffered = AccountApi.retrofitService.login(LoginProperty(email, password))
                val token = getJWTDeffered.await()
                val edit = sharedPreferences.edit()
                edit.putString("token", token)
                edit.putString("password", password)
                edit.putString("email", email)
                edit.apply()
                _loggedIn.value = true
            } catch (t: Throwable){
                _errorMessage.value = t.message
            }
        }
    }
}
