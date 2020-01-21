package be.grasmaaier.kolveniershof

import android.widget.LinearLayout
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.recyclerview.widget.RecyclerView

class RowItemViewHolder(val rowView: LinearLayout): RecyclerView.ViewHolder(rowView)

class DayItemViewHolder(val dayView: ConstraintLayout): RecyclerView.ViewHolder(dayView)

class GameItemViewHolder(val gameView: ConstraintLayout): RecyclerView.ViewHolder(gameView)