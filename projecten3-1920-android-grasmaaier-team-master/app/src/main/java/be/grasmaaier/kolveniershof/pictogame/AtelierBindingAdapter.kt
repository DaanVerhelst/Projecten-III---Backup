package be.grasmaaier.kolveniershof.pictogame

import android.view.LayoutInflater
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import androidx.core.net.toUri
import androidx.recyclerview.widget.RecyclerView
import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.schema.AtelierPropery
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.RowItemViewHolder
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions

class AtelierBindingAdapter : RecyclerView.Adapter<RowItemViewHolder>() {
    var data =  listOf<AtelierPropery>()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun getItemCount(): Int {
        return data.size
    }

    override fun onBindViewHolder(holder: RowItemViewHolder, position: Int) {
        val item = data[position]
        bindAtelierImageOnId((holder.rowView.getChildAt(0) as ImageView), item.atelierID)
        (holder.rowView.getChildAt(1) as TextView).text = String.format("%s", item.naam)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RowItemViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val view = layoutInflater
            .inflate(R.layout.atelier_row_item_view, parent, false) as LinearLayout
        return RowItemViewHolder(view)
    }


    private fun bindAtelierImageOnId(imgView: ImageView, imageId:Int?){
        imageId?.let {
            val  imgUri = String.format("%sAtelier/%d/Picto",  BuildConfig.BASE_URL, imageId).toUri()
            Glide.with(imgView.context)
                .load(imgUri)
                .apply(
                    RequestOptions().placeholder(R.drawable.default_user)
                        .error(R.drawable.default_user)
                ).into(imgView)
        }
    }
}