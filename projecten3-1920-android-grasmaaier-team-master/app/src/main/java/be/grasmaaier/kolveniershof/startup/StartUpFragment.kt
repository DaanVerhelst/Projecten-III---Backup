package be.grasmaaier.kolveniershof.startup

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.navigation.fragment.findNavController
import be.grasmaaier.kolveniershof.MainActivity
import be.grasmaaier.kolveniershof.R
import kotlinx.android.synthetic.main.activity_start_up.*

class StartUpFragment : Fragment() {

    private lateinit var viewModel: StartUpViewModel
    private lateinit var viewModelFactory: StartUpViewModelFactory

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {



        return inflater.inflate(R.layout.fragment_start_up, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        var sharedPrefs = activity?.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE)

        sharedPrefs?.let {
            viewModelFactory = StartUpViewModelFactory(it)
            viewModel = ViewModelProviders.of(this, viewModelFactory).get(StartUpViewModel::class.java)
            viewModel.loggedIn.observe(this, Observer { bool ->
                if (bool){
                    startActivity(Intent(context, MainActivity::class.java))
                } else {
                    loginNavHostFragment.findNavController().navigate(R.id.action_startUpFragment2_to_loginFragment)
                }
            })

            viewModel.errorMessage.observe(this, Observer { message ->
                Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
            })

            try {
                viewModel.getToken()
            }catch (t: Throwable){
                loginNavHostFragment.findNavController().navigate(R.id.action_startUpFragment2_to_loginFragment)
            }


        }
    }
}
