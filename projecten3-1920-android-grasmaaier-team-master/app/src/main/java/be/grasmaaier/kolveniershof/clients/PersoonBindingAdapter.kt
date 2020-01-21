package be.grasmaaier.kolveniershof.clients

import android.os.Bundle
import android.view.LayoutInflater
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import androidx.core.net.toUri
import androidx.navigation.Navigation
import androidx.recyclerview.widget.RecyclerView
import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.RowItemViewHolder
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.bumptech.glide.request.RequestOptions

class PersoonBindingAdapter : RecyclerView.Adapter<RowItemViewHolder>() {
    var data =  listOf<PersoonProperty>()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun getItemCount(): Int {
        return data.size
    }

    override fun onBindViewHolder(holder: RowItemViewHolder, position: Int) {
        val item = data[position]
        val bundle = Bundle()
        bundle.putParcelable("person", item)
        holder.rowView.setOnClickListener(Navigation.createNavigateOnClickListener(R.id.action_clientListFragment_to_clientWeekOverviewFragment, bundle))

        bindClientImageOnId((holder.rowView.getChildAt(0) as ImageView), item.id)
        (holder.rowView.getChildAt(1) as TextView).text = String.format("%s%n%s", item.voornaam, item.familienaam)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RowItemViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val view = layoutInflater
            .inflate(R.layout.person_row_item_view, parent, false) as LinearLayout
        return RowItemViewHolder(view)
    }


    private fun bindClientImageOnId(imgView: ImageView, imageId:Long?){
        imageId?.let {
            val  imgUri = String.format("%sPersoon/profilepic/%d",  BuildConfig.BASE_URL, imageId).toUri()
            Glide.with(imgView.context)
                .load(imgUri)
                .apply(
                    RequestOptions().placeholder(R.drawable.default_user)
                        .error(R.drawable.default_user).diskCacheStrategy(DiskCacheStrategy.RESOURCE)
                )
                .into(imgView)
        }
    }
}