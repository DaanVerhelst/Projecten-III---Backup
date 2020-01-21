package be.grasmaaier.kolveniershof.schema

import android.view.LayoutInflater
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TableLayout
import android.widget.TableRow
import android.widget.Toast
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.core.net.toUri
import androidx.recyclerview.widget.RecyclerView
import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.DayItemViewHolder
import be.grasmaaier.kolveniershof.R
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.bumptech.glide.request.RequestOptions
import java.sql.Time

class DagBindingAdapter: RecyclerView.Adapter<DayItemViewHolder>(), DataAdapter {
    var dagenDrawables : List<Int> = listOf(R.drawable.maandagbanner, R.drawable.dinsdagbanner, R.drawable.woensdagbanner, R.drawable.donderdagbanner, R.drawable.vrijdagbanner)

    companion object {
        val MAX_AMOUT_ACTIVITIES = 6
        val NOON_TIME = Time.valueOf("12:30:00")
    }

    var data =  listOf<DagProperty>()
        set(value) {
            field = value
            notifyDataSetChanged()
        }


    override fun setDagData(data : List<DagProperty>){
        this.data = data
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DayItemViewHolder {
        val layoutInflater = LayoutInflater.from(parent.context)
        val view = layoutInflater
            .inflate(R.layout.client_detail_day_view, parent, false) as ConstraintLayout
        return DayItemViewHolder(view)
    }

    override fun getItemCount(): Int {
        return data.size
    }

    override fun onBindViewHolder(holder: DayItemViewHolder, position: Int) {
        val item = data[position]
        var vm_counter = 0; var nm_counter = 0;
        var imgV : ImageView

        item.ateliers.forEach{
            if (it.parsedStart.before(NOON_TIME)){
                if (vm_counter > MAX_AMOUT_ACTIVITIES){
                    Toast.makeText(holder.dayView.context, String.format("Er waren meer dan %d activiteiten!", MAX_AMOUT_ACTIVITIES), Toast.LENGTH_SHORT).show()
                } else {
                    if (vm_counter % 2 == 0){
                        imgV = holder.dayView.findViewById<TableLayout>(R.id.day_view_tableLayout).findViewById<TableRow>(R.id.day_view_vm_row1).getVirtualChildAt(vm_counter/2) as ImageView
                        bindClientImageOnId(imgV, it.atelierID)
                    } else {
                        imgV = holder.dayView.findViewById<TableLayout>(R.id.day_view_tableLayout).findViewById<TableRow>(R.id.day_view_vm_row2).getVirtualChildAt(vm_counter/2) as ImageView
                        bindClientImageOnId(imgV, it.atelierID)
                    }
                    vm_counter += 1
                }
            } else {
                if (nm_counter > MAX_AMOUT_ACTIVITIES){
                    Toast.makeText(holder.dayView.context, String.format("Er waren meer dan %d activiteiten!", MAX_AMOUT_ACTIVITIES), Toast.LENGTH_SHORT).show()
                } else {
                    if (nm_counter % 2 == 0){
                        imgV = holder.dayView.findViewById<TableLayout>(R.id.day_view_tableLayout).findViewById<TableRow>(R.id.day_view_nm_row1).getVirtualChildAt(nm_counter/2) as ImageView
                        bindClientImageOnId(imgV, it.atelierID)
                    } else {
                        imgV = holder.dayView.findViewById<TableLayout>(R.id.day_view_tableLayout).findViewById<TableRow>(R.id.day_view_nm_row2).getVirtualChildAt(nm_counter/2) as ImageView
                        bindClientImageOnId(imgV, it.atelierID)
                    }
                    nm_counter += 1
                }
            }
        }
        (holder.dayView.getViewById(R.id.day_view_day_image) as ImageView).setImageResource(dagenDrawables[position])
    }

    private fun bindClientImageOnId(imgView: ImageView, imageId:Int?){
        imageId?.let {
            val  imgUri = String.format("%sAtelier/%d/Picto",  BuildConfig.BASE_URL, imageId).toUri()
            Glide.with(imgView.context)
                .load(imgUri)
                .apply(
                    RequestOptions().placeholder(R.drawable.blanco)
                        .error(R.drawable.blanco).diskCacheStrategy(DiskCacheStrategy.RESOURCE)
                )
                .into(imgView)
        }
    }
}