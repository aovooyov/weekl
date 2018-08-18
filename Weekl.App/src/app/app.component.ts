import { Component, OnInit } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'app',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {


  constructor(
    private title: Title,
    private meta: Meta
  ) {
    
  }

  ngOnInit() {
    // this.title.setTitle('weekl - твой поток информации');
    // this.meta.addTag({name: 'description', content: 'что проихсодит'});
  }
}
