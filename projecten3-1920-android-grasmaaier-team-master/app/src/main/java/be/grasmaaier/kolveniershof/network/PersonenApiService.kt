package be.grasmaaier.kolveniershof.network

import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.clients.PersoonProperty
import com.jakewharton.retrofit2.adapter.kotlin.coroutines.CoroutineCallAdapterFactory
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Deferred
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory
import retrofit2.http.GET

private val httpClient = OkHttpClient.Builder()
    .addInterceptor(AuthInterceptor()).build()

private val moshi = Moshi.Builder()
    .add(KotlinJsonAdapterFactory())
    .build()

private val retrofitJsonMap = Retrofit.Builder()
    .client(httpClient)
    .addConverterFactory(MoshiConverterFactory.create(moshi))
    .addCallAdapterFactory(CoroutineCallAdapterFactory())
    .baseUrl(BuildConfig.BASE_URL)
    .build()


interface PersonenApiService {
    @GET("Persoon/clienten")
    fun getClienten():
            Deferred<List<PersoonProperty>>

    @GET("Persoon/begeleiders")
    fun getBegeleiders():
            Deferred<List<PersoonProperty>>
}

object PersonenApi {
    val retrofitService : PersonenApiService by lazy {
        retrofitJsonMap.create(PersonenApiService::class.java)
    }
}