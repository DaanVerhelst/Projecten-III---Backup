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
import org.junit.*

class StartUpActivityTest {

    @get:Rule
    val activityRule = object :ActivityTestRule<StartUpActivity>(StartUpActivity::class.java){
        override fun beforeActivityLaunched() {
            ApplicationProvider.getApplicationContext<Context>().getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
            super.beforeActivityLaunched()
        }
    }

    @Before
    fun before() {
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
        }
    }

    @After
    fun after(){
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
        }
    }

    @Test
    fun loginAccesNotGranted() {
        Espresso.onView(ViewMatchers.withId(R.id.txt_login_email)).perform(ViewActions.typeText("dzjqodijq"), ViewActions.closeSoftKeyboard())
        Espresso.onView(ViewMatchers.withId(R.id.txt_login_password)).perform(ViewActions.typeText("dzjqodijq"), ViewActions.closeSoftKeyboard())
        Espresso.onView(ViewMatchers.withId(R.id.btn_login)).perform(ViewActions.click())
        Espresso.onView(ViewMatchers.withText("Wie ben je?")).check(ViewAssertions.doesNotExist())
    }


    @Test
    fun loginAccesGranted() {
        Espresso.onView(ViewMatchers.withId(R.id.txt_login_email)).perform(ViewActions.typeText("Tycho.Altink@mail.be"), ViewActions.closeSoftKeyboard())
        Espresso.onView(ViewMatchers.withId(R.id.txt_login_password)).perform(ViewActions.typeText("admin123"), ViewActions.closeSoftKeyboard())
        Espresso.onView(ViewMatchers.withId(R.id.btn_login)).perform(ViewActions.click())
        Espresso.onView(ViewMatchers.withText("Wie ben je?")).check(matches(ViewMatchers.isDisplayed()))
    }
}