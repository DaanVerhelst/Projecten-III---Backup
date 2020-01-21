package be.grasmaaier.kolveniershof.network

import be.grasmaaier.kolveniershof.network.AccountApi.sharedPreferences
import okhttp3.Interceptor
import okhttp3.Response

class AuthInterceptor : Interceptor {

    override fun intercept(chain: Interceptor.Chain): Response {
        var request = chain.request()

        var token = sharedPreferences?.getString("token", "")
        if (token != ""){
            request = request?.newBuilder()
                ?.addHeader("Authorization", String.format("Bearer %s", token))
                ?.build()
        }
        return chain.proceed(request)
    }
}