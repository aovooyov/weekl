import { Component, OnInit, ViewChild } from '@angular/core';
import { FilterComponent } from './components/filter.component';

@Component({
  selector: 'app',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  @ViewChild(FilterComponent) filter: FilterComponent;

  constructor() {
    
  }

  ngOnInit() {
  }

  openFilter(e: Event) {
    e.preventDefault();
    this.filter.sidebar.toggle();
  }
}
