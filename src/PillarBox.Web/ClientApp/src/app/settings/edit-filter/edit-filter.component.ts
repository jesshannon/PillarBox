import { Observable } from 'rxjs/Observable';
import { Component, OnInit } from '@angular/core';

// common fields offered as autocomplete
const fields = ['Apparently-To', 'Bcc', 'CLIENT_HOST', 'CLIENT_IP', 'Cc', 'Comments', 'Content-Transfer-Encoding', 'Content-Type', 'Date', 'Errors-To', 'From', 'From', 'INBOX', 'In-Reply-To', 'Message-Id', 'Mime-Version', 'Newsgroups', 'Organization', 'Priority', 'Received', 'References', 'Reply-To', 'Sender', 'Subject', 'To', 'X-Confirm-Reading-To', 'X-Distribution', 'X-Errors-To', 'X-Mailer', 'X-PMFLAGS', 'X-Priority', 'X-Sender', 'X-UIDL', 'X-headers'];

@Component({
  selector: 'app-edit-filter',
  templateUrl: './edit-filter.component.html',
  styleUrls: ['./edit-filter.component.scss']
})
export class EditFilterComponent implements OnInit {

  action: string = 'forward';

  rule = {
    name: 'my filter',
    filters: [
      {
        id: '3c423ab3f',
        field: 'CLIENT_IP',
        pattern: '192.168.*',
        isRegularExpression: false
      },
      {
        id: '3c423ab3f',
        field: 'To',
        pattern: '.*@example\\.(com|net)',
        isRegularExpression: true
      }
    ],
    actionWebHook: {
      targetUrl: '',
      postTemplate: ''
    },
    actionForward: {
      recipient: ''
    }
  };

  constructor() { }

  ngOnInit() {
  }

  fieldSuggest = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .map(term => term.length < 1 ? []
        : fields.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));


  addCondition() {
    this.rule.filters.push({
      id: '', field: '', pattern: '', isRegularExpression: false
    });
  }

  removeCondition(index: number) {
    this.rule.filters.splice(index, 1);
  }

}
