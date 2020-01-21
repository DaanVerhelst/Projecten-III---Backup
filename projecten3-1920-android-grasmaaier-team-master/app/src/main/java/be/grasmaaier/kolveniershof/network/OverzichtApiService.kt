package be.grasmaaier.kolveniershof.network

import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.schema.DagProperty
import com.jakewharton.retrofit2.adapter.kotlin.coroutines.CoroutineCallAdapterFactory
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Deferred
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory
import retrofit2.http.GET
import retrofit2.http.Path

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


interface OverzichtApiService {
    @GET("Dag/week/{start}/client/{id}")
    fun getWeekVoorClient(@Path("start") start : String, @Path("id") id : Long):
            Deferred<List<DagProperty>>
}

object OverzichtApi {
    val retrofitService : OverzichtApiService by lazy {
        retrofitJsonMap.create(OverzichtApiService::class.java)
    }
}