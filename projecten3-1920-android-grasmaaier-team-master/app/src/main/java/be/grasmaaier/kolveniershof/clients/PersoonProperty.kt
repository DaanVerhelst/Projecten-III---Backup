package be.grasmaaier.kolveniershof.clients

import android.os.Parcel
import android.os.Parcelable
import be.grasmaaier.kolveniershof.database.DatabaseClient

data class PersoonProperty(
    val id: Long,
    val voornaam: String,
    val familienaam: String
) : Parcelable {
    constructor(parcel: Parcel) : this(
        parcel.readLong(),
        parcel.readString() ?: "",
        parcel.readString() ?:""
    ){}

    override fun writeToParcel(parcel: Parcel, flags: Int) {
        parcel.writeLong(id)
        parcel.writeString(voornaam)
        parcel.writeString(familienaam)
    }

    override fun describeContents(): Int {
        return 0
    }

    companion object CREATOR : Parcelable.Creator<PersoonProperty> {
        override fun createFromParcel(parcel: Parcel): PersoonProperty {
            return PersoonProperty(parcel)
        }

        override fun newArray(size: Int): Array<PersoonProperty?> {
            return arrayOfNulls(size)
        }
    }

}

fun List<PersoonProperty>.asDatabaseModel(): List<DatabaseClient> {
    return map {
        DatabaseClient(
            id = it.id,
            voornaam = it.voornaam,
            familienaam = it.familienaam
        )
    }
}