package be.grasmaaier.kolveniershof.clients

import android.os.Bundle
import android.view.*
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders

import be.grasmaaier.kolveniershof.databinding.FragmentClientListBinding
import androidx.recyclerview.widget.GridLayoutManager
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.database.KolveniersHofDatabase


class ClientListFragment : Fragment() {
    private  lateinit var viewModel: ClientViewModel
    private  lateinit var viewModelFactory: ClientViewModelFactory

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {


        val binding: FragmentClientListBinding =  DataBindingUtil.inflate(inflater,
            R.layout.fragment_client_list, container, false)
        val adapter = PersoonBindingAdapter()
        binding.sleepList.adapter = adapter
        binding.sleepList.layoutManager = GridLayoutManager(this.context, 4)

        val database = KolveniersHofDatabase.getInstance(requireContext())
        viewModelFactory = ClientViewModelFactory(database)
        viewModel = ViewModelProviders.of(this, viewModelFactory).get(ClientViewModel::class.java)

        viewModel.clienten.observe(viewLifecycleOwner, Observer {
            it?.let{
                adapter.data = it
            }
        })
        return binding.root
    }
}

