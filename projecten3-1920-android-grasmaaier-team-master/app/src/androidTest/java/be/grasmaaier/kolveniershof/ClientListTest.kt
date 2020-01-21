package be.grasmaaier.kolveniershof

import android.app.Activity
import androidx.recyclerview.widget.RecyclerView
import androidx.test.espresso.Espresso.onView
import androidx.test.espresso.action.ViewActions.click
import androidx.test.espresso.assertion.ViewAssertions.doesNotExist
import androidx.test.espresso.assertion.ViewAssertions.matches
import androidx.test.espresso.contrib.RecyclerViewActions
import androidx.test.espresso.contrib.RecyclerViewActions.actionOnItem
import androidx.test.espresso.contrib.RecyclerViewActions.actionOnItemAtPosition
import androidx.test.espresso.matcher.ViewMatchers.*
import androidx.test.rule.ActivityTestRule
import org.hamcrest.Matchers.allOf
import org.junit.Before
import org.junit.Rule
import org.junit.Test

class ClientListTest {

    @get:Rule
    val activityRule = ActivityTestRule(MainActivity::class.java)

    @Before
    fun before() {
        activityRule.activity.apply {
            this.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE).edit().remove("token").commit()
        }
    }

    //opening app happens automatically so does not need a test!

    @Test
    fun login() {
    //login to the app - apparently already logged in?! so token removal does not work?
    }



    @Test
    fun checkWhetherClientDetailExists() {
        //check whether the clientdetail view exists at this point
        onView(withId(R.id.clientDetailFragment)).check(doesNotExist())
    }

    @Test
    fun selectAClient() {
        //fails for some reason, not sure why?
       onView(withId(R.id.sleep_list))
            .perform(
                actionOnItem<RecyclerView.ViewHolder>(
                hasDescendant(withText("Romein")), click())
            )
        //onView(allOf(withId(R.id.sleep_list), withText("Romein"))).perform(click())
        onView(withId(R.id.clientDetailFragment)).check(matches(isDisplayed()))
        //select a client with a certain name, getbyID to get to client cards, get the one with a certain text.
        //see if it takes you to the weekview (check if text in header is same as clicked text)
    }

    /*@Test
    fun testRecyclerView() {
        //test whether the recycler view operates correctly.
         onView(allOf(withId(R.id.sleep_list),
           withParent(withId(R.id.clientListFragment)),
           isDisplayed())).perform(actionOnItemAtPosition<RecyclerView.ViewHolder>(3, click()))
        // onView(withRecyclerView) check https://stackoverflow.com/questions/31394569/how-to-assert-inside-a-recyclerview-in-espresso

    }*/

    @Test
    fun checkWhetherClientListExists() {
        onView(withId(R.id.clientListFragment)).check(doesNotExist())
    }
    @Test
    fun switchViews() {
    //switch from landscape to portrait, check if views change properly
        onView(withId(R.id.switch_button)).perform(click())
        onView(withId(R.id.DayTable)).check(matches(isDisplayed()))
        onView(withId(R.id.WeekTable)).check(doesNotExist())
        onView(withId(R.id.switch_button)).perform(click())
        onView(withId(R.id.WeekTable)).check(matches(isDisplayed()))
        onView(withId(R.id.DayTable)).check(doesNotExist())
    }

}