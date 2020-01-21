package be.grasmaaier.kolveniershof

import android.app.Activity
import android.content.Context
import androidx.test.core.app.ApplicationProvider
import androidx.test.espresso.Espresso
import androidx.test.espresso.action.ViewActions
import androidx.test.espresso.assertion.ViewAssertions
import androidx.test.espresso.assertion.ViewAssertions.matches
import androidx.test.espresso.matcher.ViewMatchers
import androidx.test.rule.ActivityTestRule
import org.junit.After
import org.junit.Before
import org.junit.Rule
import org.junit.Test

class StartUpAcitivityWithWrongTokenTest {

    @get:Rule
    val activityRule = object : ActivityTestRule<StartUpActivity>(StartUpActivity::class.java){
        override fun beforeActivityLaunched() {
            ApplicationProvider.getApplicationContext<Context>().getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().putString("token", "").commit()
            super.beforeActivityLaunched()
        }
    }

    @Before
    fun before() {
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().putString("token", "").commit()
        }
    }

    @After
    fun after(){
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
        }
    }

    @Test
    fun redirectedToLoginPage() {
        Espresso.onView(ViewMatchers.withId(R.id.btn_login)).check(matches(ViewMatchers.isDisplayed()))
        Espresso.onView(ViewMatchers.withText("Wie ben je?")).check(ViewAssertions.doesNotExist())
    }
}