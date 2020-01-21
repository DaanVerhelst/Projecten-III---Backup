package be.grasmaaier.kolveniershof.schema

import be.grasmaaier.kolveniershof.clients.PersoonProperty
import java.sql.Time

data class AtelierPropery (
    val atelierID : Int,
    val naam : String,
    val start: String,
    val end: String,
    val clienten: List<PersoonProperty>?,
    val begeleiders: List<PersoonProperty>?
) {
    val parsedStart : Time
        get(){
            val timeFormat = Time.valueOf(start)
            return timeFormat
        }
}