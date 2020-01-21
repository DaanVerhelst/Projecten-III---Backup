package be.grasmaaier.kolveniershof.network

import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.schema.AtelierPropery
import com.jakewharton.retrofit2.adapter.kotlin.coroutines.CoroutineCallAdapterFactory
import com.squareup.moshi.Moshi
import com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory
import kotlinx.coroutines.Deferred
import retrofit2.Retrofit
import retrofit2.converter.moshi.MoshiConverterFactory
import retrofit2.http.GET

private val moshi = Moshi.Builder()
    .add(KotlinJsonAdapterFactory())
    .build()

private val retrofitJsonMap = Retrofit.Builder()
    .addConverterFactory(MoshiConverterFactory.create(moshi))
    .addCallAdapterFactory(CoroutineCallAdapterFactory())
    .baseUrl(BuildConfig.BASE_URL)
    .build()


interface AteliersApiService {
    @GET("Atelier")
    fun getAteliers():
            Deferred<List<AtelierPropery>>
}

object AtelierApi {
    val retrofitService : AteliersApiService by lazy {
        retrofitJsonMap.create(AteliersApiService::class.java)
    }
}