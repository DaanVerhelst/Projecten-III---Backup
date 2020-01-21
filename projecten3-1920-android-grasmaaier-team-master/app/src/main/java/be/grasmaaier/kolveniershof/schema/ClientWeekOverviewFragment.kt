package be.grasmaaier.kolveniershof.schema

import android.os.Bundle
import android.view.*
import androidx.appcompat.app.AppCompatActivity
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.LinearLayoutManager
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.clients.PersoonProperty
import be.grasmaaier.kolveniershof.databinding.FragmentClientWeekOverviewBinding
import android.content.res.Configuration
import android.widget.Button
import android.widget.Toast
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.navigation.fragment.findNavController
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase


class ClientWeekOverviewFragment : Fragment() {
    private lateinit var viewModel: DetailViewModel
    private lateinit var viewModelFactory: DetailViewModelFactory
    private lateinit var binding: FragmentClientWeekOverviewBinding
    private lateinit var pagerAdapter: DataAdapter
    private lateinit var recyclerAdapter: DataAdapter
    private lateinit var persoon:PersoonProperty
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        persoon = arguments?.getParcelable<PersoonProperty>("person") as PersoonProperty
        val database = KolveniersHofDatabase.getInstance(requireContext())
        viewModelFactory = DetailViewModelFactory(persoon, database)
        viewModel = ViewModelProviders.of(this, viewModelFactory).get(DetailViewModel::class.java)

        binding = DataBindingUtil.inflate(
            inflater, R.layout.fragment_client_week_overview, container, false
        )

        makeAdapter(binding)

        viewModel.week.observe(viewLifecycleOwner, Observer {
            it?.let{
                recyclerAdapter.setDagData(it)
                pagerAdapter.setDagData(it)
            }
        })

        viewModel.error.observe(viewLifecycleOwner, Observer {
            Toast.makeText(context, it, Toast.LENGTH_LONG).show()
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
        val bundle = Bundle()
        bundle.putParcelable("person", this.persoon)
        return when (item.itemId) {

            R.id.validateFragment -> {
               findNavController().navigate(R.id.action_clientWeekOverviewFragment_to_clientGameFragment,bundle)

                return true
            } else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onConfigurationChanged(newConfig: Configuration) {
        super.onConfigurationChanged(newConfig)
        makeAdapter(binding)
    }

    private fun makeAdapter(binding : FragmentClientWeekOverviewBinding){
        recyclerAdapter = DagBindingAdapter()
        binding.dayList.adapter = recyclerAdapter as DagBindingAdapter
        binding.dayList.layoutManager = SpanningLinearLayoutManager(context, LinearLayoutManager.HORIZONTAL, false)
        pagerAdapter = DagPagerAdapter(context!!)
        binding.dayPager.adapter = pagerAdapter as DagPagerAdapter
    }
}
