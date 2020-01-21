package be.grasmaaier.kolveniershof


import be.grasmaaier.kolveniershof.login.LoginViewModel
import org.junit.Assert
import org.junit.Test
import android.content.SharedPreferences
import org.junit.Before
import org.mockito.Mockito
import org.junit.runner.Request.method







class LoginTest {

    @Test(expected = AssertionError::class)
    fun `login method gets wrong values, receives an error from backend`() {
        val sharedPrefs = Mockito.mock(SharedPreferences::class.java)
        val lv = LoginViewModel(sharedPrefs);
        Assert.assertEquals("Bad Request", lv.login("verkeerd@nee.nee", "nee"))
    }
}