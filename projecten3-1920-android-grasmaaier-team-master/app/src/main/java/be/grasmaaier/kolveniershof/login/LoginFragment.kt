package be.grasmaaier.kolveniershof.login

import android.app.Activity
import android.content.Intent
import androidx.lifecycle.ViewModelProviders
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.databinding.DataBindingUtil
import androidx.lifecycle.Observer
import be.grasmaaier.kolveniershof.MainActivity
import be.grasmaaier.kolveniershof.R
import be.grasmaaier.kolveniershof.databinding.LoginFragmentBinding
import be.grasmaaier.kolveniershof.startup.StartUpViewModelFactory
import kotlinx.android.synthetic.main.login_fragment.*

class LoginFragment : Fragment() {

    private lateinit var viewModel: LoginViewModel
    private lateinit var viewModelFactory: StartUpViewModelFactory
    private lateinit var binding: LoginFragmentBinding

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        binding = DataBindingUtil.inflate(inflater, R.layout.login_fragment, container, false)
        return binding.root
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        var sharedPrefs = activity?.getSharedPreferences("KolveniersHof", Activity.MODE_PRIVATE)

        sharedPrefs?.let {
            viewModelFactory = StartUpViewModelFactory(it)
            viewModel = ViewModelProviders.of(this, viewModelFactory).get(LoginViewModel::class.java)
            btn_login.setOnClickListener{
                viewModel.login(binding.txtLoginEmail.text.toString(), binding.txtLoginPassword.text.toString())
            }

            viewModel.loggedIn.observe(this, Observer { bool ->
                if (bool){
                    startActivity(Intent(context, MainActivity::class.java))
                }
            })

            viewModel.errorMessage.observe(this, Observer { message  ->
                Toast.makeText(context, message, Toast.LENGTH_SHORT).show()
            })
        }
    }

}
