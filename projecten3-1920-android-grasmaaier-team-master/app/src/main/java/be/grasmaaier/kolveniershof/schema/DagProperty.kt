package be.grasmaaier.kolveniershof.schema

import android.view.ViewParent
import java.text.SimpleDateFormat
import java.util.*

data class DagProperty (
    val date: String,
    val ateliers: List<AtelierPropery>
        ){
    val dateParsed: Date
        get(){
            val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            return dateFormat.parse(date)
        }
}