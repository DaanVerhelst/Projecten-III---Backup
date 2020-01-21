package be.grasmaaier.kolveniershof

import android.app.Activity
import androidx.test.espresso.Espresso.onView
import androidx.test.espresso.assertion.ViewAssertions.matches
import androidx.test.espresso.matcher.ViewMatchers.isDisplayed
import androidx.test.espresso.matcher.ViewMatchers.withText
import androidx.test.rule.ActivityTestRule
import org.junit.Before
import org.junit.Rule
import org.junit.Test

class MainActivityTest {

    @get:Rule
    val activityRule = ActivityTestRule(MainActivity::class.java)

    @Before
    fun before() {
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
        }
    }

    @Test
    fun firstPage() {
        onView(withText("Wie ben je?")).check(matches(isDisplayed()))
    }
}
