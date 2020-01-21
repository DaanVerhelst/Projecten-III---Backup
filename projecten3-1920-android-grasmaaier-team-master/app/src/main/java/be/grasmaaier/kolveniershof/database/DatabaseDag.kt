package be.grasmaaier.kolveniershof.database

import androidx.room.Entity
import androidx.room.ForeignKey

@Entity(tableName = "dag_atelier_client_table",
    primaryKeys = ["datum", "client"],
    foreignKeys = [
        ForeignKey(
            entity = DatabaseClient::class,
            parentColumns = ["id"],
            childColumns = ["client"],
            onDelete = ForeignKey.CASCADE
        )
    ]
)
data class DatabaseDagAtelierClient constructor(
    var datum: String = "",
    var client: Long = 0,
    var data: String = ""
)
