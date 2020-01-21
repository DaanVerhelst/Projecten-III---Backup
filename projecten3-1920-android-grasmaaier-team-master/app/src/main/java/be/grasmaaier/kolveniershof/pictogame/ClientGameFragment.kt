package be.grasmaaier.kolveniershof.pictogame

import android.content.res.Configuration
import android.os.Bundle
import android.view.*
import android.widget.Button
import androidx.appcompat.app.AppCompatActivity
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.LinearLayoutManager
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.clients.PersoonProperty
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase
import be.grasmaaier.kolveniershof.databinding.FragmentClientGameBinding
import be.grasmaaier.kolveniershof.schema.*

class ClientGameFragment : Fragment() {
    private lateinit var viewModel: DetailViewModel
    private lateinit var viewModelFactory: DetailViewModelFactory
    private lateinit var binding: FragmentClientGameBinding
    private lateinit var pagerAdapter: DataAdapter
    private lateinit var recyclerAdapter: DataAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {


        var persoon = arguments?.getParcelable<PersoonProperty>("person") as PersoonProperty
        val database = KolveniersHofDatabase.getInstance(requireContext())
         viewModelFactory = DetailViewModelFactory(persoon,database)
         viewModel = ViewModelProviders.of(this, viewModelFactory).get(DetailViewModel::class.java)

        binding = DataBindingUtil.inflate(
            inflater, R.layout.fragment_client_game, container, false
        )

        makeAdapter(binding)

        viewModel.week.observe(viewLifecycleOwner, Observer {
            it?.let {
                recyclerAdapter.setDagData(it)
                pagerAdapter.setDagData(it)
            }
        })

        (activity as AppCompatActivity).supportActionBar?.title = String.format("%s %s", viewModel.persoon.voornaam, viewModel.persoon.familienaam)
        setHasOptionsMenu(true)
        val orientation = resources.configuration.orientation
        if (orientation == Configuration.ORIENTATION_PORTRAIT) {
            (binding.root as ConstraintLayout).findViewById<Button>(R.id.day_pager_next).setOnClickListener{
                binding.dayPager.currentItem++
            }

            (binding.root as ConstraintLayout).findViewById<Button>(R.id.day_pager_previous).setOnClickListener{
                binding.dayPager.currentItem--
            }
        }


        return binding.root
    }

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        super.onCreateOptionsMenu(menu, inflater)
        inflater?.inflate(R.menu.overflow_menu, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.validateFragment -> {

                return true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onConfigurationChanged(newConfig: Configuration) {
        super.onConfigurationChanged(newConfig)
        makeAdapter(binding)
    }

    private fun makeAdapter(binding: FragmentClientGameBinding) {
        recyclerAdapter = GameBindingAdapter()
        binding.dayList.adapter = recyclerAdapter as GameBindingAdapter
        binding.dayList.layoutManager =
            SpanningLinearLayoutManager(context, LinearLayoutManager.HORIZONTAL, false)
        pagerAdapter = gamePagerAdapter(context!!)
        binding.dayPager.adapter = pagerAdapter as gamePagerAdapter
    }
}
