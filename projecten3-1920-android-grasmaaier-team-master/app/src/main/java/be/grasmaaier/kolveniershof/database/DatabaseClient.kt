package be.grasmaaier.kolveniershof.database

import androidx.room.Entity
import androidx.room.PrimaryKey
import be.grasmaaier.kolveniershof.clients.PersoonProperty

@Entity(tableName = "client_table")
data class DatabaseClient constructor(
    @PrimaryKey
    val id: Long,
    val voornaam: String,
    val familienaam: String
)

fun List<DatabaseClient>.asDomainModel(): List<PersoonProperty> {
    return map {
        PersoonProperty(
            id = it.id,
            voornaam = it.voornaam,
            familienaam = it.familienaam
        )
    }
}