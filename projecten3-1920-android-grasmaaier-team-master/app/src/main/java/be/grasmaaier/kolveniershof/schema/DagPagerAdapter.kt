package be.grasmaaier.kolveniershof.schema

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TableLayout
import android.widget.TableRow
import android.widget.Toast
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.core.net.toUri
import androidx.databinding.DataBindingUtil
import androidx.viewpager.widget.PagerAdapter
import be.grasmaaier.kolveniershof.BuildConfig
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.databinding.FragmentDagBinding
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.bumptech.glide.request.RequestOptions
import java.sql.Time

class DagPagerAdapter(context: Context): PagerAdapter(), DataAdapter {

    companion object {
        var DAGEN_DRAWABLES : List<Int> = listOf(R.drawable.maandagbanner, R.drawable.dinsdagbanner, R.drawable.woensdagbanner, R.drawable.donderdagbanner, R.drawable.vrijdagbanner)
        val NOON_TIME = Time.valueOf("12:30:00")
        val MAX_AMOUT_ACTIVITIES = 5
    }

    override fun setDagData(data : List<DagProperty>){
        this.data = data
    }

    var data =  listOf<DagProperty>()
        set(value) {
            field = value
            notifyDataSetChanged()
        }

    override fun instantiateItem(collection: ViewGroup, position: Int): Any {
        val inflater = LayoutInflater.from(context)
        var binding = DataBindingUtil.inflate<FragmentDagBinding>(inflater, R.layout.fragment_dag, collection, false)

        var content = binding.dayFragmentContent as ConstraintLayout

        var vm_counter = 0; var nm_counter = 0;
        var imgV : ImageView


        data[position].ateliers.forEach{
            if (it.parsedStart.before(NOON_TIME)){
                if (vm_counter > MAX_AMOUT_ACTIVITIES){
                    Toast.makeText(context, String.format("Er waren meer dan %d activiteiten!",
                        MAX_AMOUT_ACTIVITIES
                    ), Toast.LENGTH_SHORT).show()
                } else {
                    imgV = content.findViewById<TableLayout>(R.id.day_view_tableLayout)!!.findViewById<TableRow>(
                        R.id.day_view_vm_row1).getVirtualChildAt(vm_counter) as ImageView
                    bindClientImageOnId(imgV, it.atelierID)
                    vm_counter += 1
                }
            } else {
                if (nm_counter > DagBindingAdapter.MAX_AMOUT_ACTIVITIES){
                    Toast.makeText(context, String.format("Er waren meer dan %d activiteiten!",
                        DagBindingAdapter.MAX_AMOUT_ACTIVITIES
                    ), Toast.LENGTH_SHORT).show()
                } else {
                    imgV = content.findViewById<TableLayout>(R.id.day_view_tableLayout)!!.findViewById<TableRow>(
                        R.id.day_view_nm_row1).getVirtualChildAt(nm_counter) as ImageView
                    bindClientImageOnId(imgV, it.atelierID)
                    nm_counter += 1
                }
            }
        }
        content.findViewById<ImageView>(R.id.day_view_day_image).setImageResource(DAGEN_DRAWABLES[position])

        collection.addView(binding.root)
        return binding.root
    }

    override fun destroyItem(collection: ViewGroup, position: Int, view: Any) {
        collection.removeView(view as View)
    }

    var context: Context = context

    override fun isViewFromObject(view: View, `object`: Any): Boolean {
        return view === `object`
    }

    override fun getCount(): Int {
        return data.size
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

