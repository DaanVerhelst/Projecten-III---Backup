package be.grasmaaier.kolveniershof.network

import android.content.SharedPreferences
import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.login.LoginProperty
import com.jakewharton.retrofit2.adapter.kotlin.coroutines.CoroutineCallAdapterFactory
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Deferred
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST

private val httpClient = OkHttpClient.Builder()
    .addInterceptor(AuthInterceptor()).build()

private val moshi = Moshi.Builder()
    .add(KotlinJsonAdapterFactory())
    .build()

private val retrofitJsonMap =
    Retrofit.Builder().client(httpClient)
        .addConverterFactory(MoshiConverterFactory.create(moshi).asLenient())
        .addCallAdapterFactory(CoroutineCallAdapterFactory())
        .baseUrl(BuildConfig.BASE_URL)
        .build()


interface AccountApiService {
    @POST("Gebruiker")
    fun login(@Body loginProperty: LoginProperty):
            Deferred<String>

    @GET("Gebruiker/checkToken")
    fun checkToken():
            Deferred<Unit>
}

object AccountApi {
    val retrofitService : AccountApiService by lazy {
        retrofitJsonMap.create(AccountApiService::class.java)
    }
    var sharedPreferences : SharedPreferences? = null
}