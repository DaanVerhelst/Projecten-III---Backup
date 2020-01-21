package be.grasmaaier.kolveniershof

import be.grasmaaier.kolveniershof.schema.DagProperty
import org.junit.Assert
import org.junit.Test
import java.lang.NullPointerException
import java.text.ParseException
import java.time.LocalDateTime
import java.time.Month
import java.time.format.DateTimeFormatter
import java.util.*

class DateConverterTest {

    private val dagProperty = DagProperty("2019-12-17T00:00:00", emptyList());

    private val dagProperty2 = DagProperty("2019-12-1''00:00:00", emptyList());

    private val dagProperty3 = DagProperty("", emptyList());

    @Test
    fun `Adding a correct date-string should return a correct date`() {
        //val dateTime = LocalDateTime.of(2019, Month.DECEMBER, 17,0,0,0)

        val dateTime = Date(119,11,17)
        // Need to expect a date, not a string, fix!!
        Assert.assertEquals(dateTime, dagProperty.dateParsed)
    }

    @Test(expected =  ParseException::class)
    fun `Adding an incorrect date-string should return an exception`(){
        dagProperty2.dateParsed
        //Assert.assertEquals("Tue Dec 17 00:00:00 GMT+1:00 2019", dagProperty2.dateParsed)
    }

    @Test(expected = ParseException::class)
    fun `Adding an empty date-string should return an exception`() {
        dagProperty3.dateParsed
    }


}