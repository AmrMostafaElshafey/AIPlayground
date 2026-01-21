import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface ServiceCatalogItem {
  id: number;
  name: string;
  category: string;
  description: string;
  eligibility: string;
  accessRules: string;
  owner: string;
}

interface PolicyDocument {
  id: number;
  title: string;
  version: string;
  summary: string;
  publishedOn: string;
}

interface RoleProfile {
  id: number;
  name: string;
  responsibilities: string;
}

interface PromptLibraryItem {
  id: number;
  track: string;
  title: string;
  description: string;
  status: string;
}

interface AccessRequest {
  id?: number;
  requesterName: string;
  role: string;
  serviceName: string;
  intendedUse: string;
  duration: string;
  status?: string;
  decisionNotes?: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  services$?: Observable<ServiceCatalogItem[]>;
  policies$?: Observable<PolicyDocument[]>;
  roles$?: Observable<RoleProfile[]>;
  prompts$?: Observable<PromptLibraryItem[]>;
  requests$?: Observable<AccessRequest[]>;

  requestDraft: AccessRequest = {
    requesterName: '',
    role: '',
    serviceName: '',
    intendedUse: '',
    duration: '',
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.services$ = this.http.get<ServiceCatalogItem[]>('http://localhost:5000/api/services');
    this.policies$ = this.http.get<PolicyDocument[]>('http://localhost:5000/api/policies');
    this.roles$ = this.http.get<RoleProfile[]>('http://localhost:5000/api/roles');
    this.prompts$ = this.http.get<PromptLibraryItem[]>('http://localhost:5000/api/prompts');
    this.requests$ = this.http.get<AccessRequest[]>('http://localhost:5000/api/requests');
  }

  submitRequest(): void {
    if (!this.requestDraft.requesterName || !this.requestDraft.serviceName) {
      return;
    }

    this.http.post<AccessRequest>('http://localhost:5000/api/requests', this.requestDraft).subscribe(() => {
      this.requestDraft = {
        requesterName: '',
        role: '',
        serviceName: '',
        intendedUse: '',
        duration: '',
      };
      this.requests$ = this.http.get<AccessRequest[]>('http://localhost:5000/api/requests');
    });
  }
}
