import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface ServiceCatalogItem {
  id: number;
  name: string;
  category: string;
  provider: string;
  description: string;
  eligibility: string;
  accessRules: string;
  owner: string;
  isApiAccess: boolean;
  studentTokenLimit: number;
  employeeTokenLimit: number;
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
  serviceId: number;
  serviceName: string;
  applicationName: string;
  intendedUse: string;
  duration: string;
  estimatedTokens: number;
  status?: string;
  decisionNotes?: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  readonly baseUrl = 'http://localhost:5000/api';
  services: ServiceCatalogItem[] = [];
  policies: PolicyDocument[] = [];
  roles: RoleProfile[] = [];
  prompts: PromptLibraryItem[] = [];
  requests: AccessRequest[] = [];

  authUser = {
    name: '',
    role: 'Guest',
  };

  loginDraft = {
    name: '',
    role: 'Student',
  };

  requestDraft: AccessRequest = {
    requesterName: '',
    role: '',
    serviceId: 0,
    serviceName: '',
    applicationName: '',
    intendedUse: '',
    duration: '',
    estimatedTokens: 0,
  };

  requestEdit: AccessRequest | null = null;

  serviceDraft: ServiceCatalogItem = {
    id: 0,
    name: '',
    category: '',
    provider: '',
    description: '',
    eligibility: '',
    accessRules: '',
    owner: '',
    isApiAccess: false,
    studentTokenLimit: 0,
    employeeTokenLimit: 0,
  };

  serviceEdit: ServiceCatalogItem | null = null;

  policyDraft: PolicyDocument = {
    id: 0,
    title: '',
    version: '',
    summary: '',
    publishedOn: new Date().toISOString(),
  };

  policyEdit: PolicyDocument | null = null;

  roleDraft: RoleProfile = {
    id: 0,
    name: '',
    responsibilities: '',
  };

  roleEdit: RoleProfile | null = null;

  promptDraft: PromptLibraryItem = {
    id: 0,
    track: '',
    title: '',
    description: '',
    status: '',
  };

  promptEdit: PromptLibraryItem | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.http.get<ServiceCatalogItem[]>(`${this.baseUrl}/services`).subscribe((data) => (this.services = data));
    this.http.get<PolicyDocument[]>(`${this.baseUrl}/policies`).subscribe((data) => (this.policies = data));
    this.http.get<RoleProfile[]>(`${this.baseUrl}/roles`).subscribe((data) => (this.roles = data));
    this.http.get<PromptLibraryItem[]>(`${this.baseUrl}/prompts`).subscribe((data) => (this.prompts = data));
    this.http.get<AccessRequest[]>(`${this.baseUrl}/requests`).subscribe((data) => (this.requests = data));
  }

  login(): void {
    if (!this.loginDraft.name) {
      return;
    }
    this.authUser = { ...this.loginDraft };
    this.requestDraft.requesterName = this.authUser.name;
    this.requestDraft.role = this.authUser.role;
  }

  logout(): void {
    this.authUser = { name: '', role: 'Guest' };
    this.requestDraft.requesterName = '';
    this.requestDraft.role = '';
  }

  get isAuthenticated(): boolean {
    return !!this.authUser.name && this.authUser.role !== 'Guest';
  }

  get isAdmin(): boolean {
    return this.authUser.role === 'Administrator';
  }

  selectService(service: ServiceCatalogItem): void {
    this.requestDraft.serviceId = service.id;
    this.requestDraft.serviceName = service.name;
  }

  getTokenLimit(service: ServiceCatalogItem): number {
    if (!service.isApiAccess) {
      return 0;
    }
    return this.authUser.role === 'Employee' ? service.employeeTokenLimit : service.studentTokenLimit;
  }

  submitRequest(): void {
    if (!this.isAuthenticated || !this.requestDraft.requesterName || !this.requestDraft.serviceId) {
      return;
    }

    this.requestDraft.role = this.authUser.role;
    this.requestDraft.requesterName = this.authUser.name;
    this.http.post<AccessRequest>(`${this.baseUrl}/requests`, this.requestDraft).subscribe(() => {
      this.requestDraft = {
        requesterName: '',
        role: '',
        serviceId: 0,
        serviceName: '',
        applicationName: '',
        intendedUse: '',
        duration: '',
        estimatedTokens: 0,
      };
      this.loadAll();
    });
  }

  editRequest(request: AccessRequest): void {
    this.requestEdit = { ...request };
  }

  saveRequest(): void {
    if (!this.requestEdit) {
      return;
    }
    this.http
      .put<AccessRequest>(`${this.baseUrl}/requests/${this.requestEdit.id}`, this.requestEdit)
      .subscribe(() => {
        this.requestEdit = null;
        this.loadAll();
      });
  }

  deleteRequest(requestId?: number): void {
    if (!requestId) {
      return;
    }
    this.http.delete(`${this.baseUrl}/requests/${requestId}`).subscribe(() => this.loadAll());
  }

  submitService(): void {
    if (!this.serviceDraft.name) {
      return;
    }
    this.http.post<ServiceCatalogItem>(`${this.baseUrl}/services`, this.serviceDraft).subscribe(() => {
      this.serviceDraft = {
        id: 0,
        name: '',
        category: '',
        provider: '',
        description: '',
        eligibility: '',
        accessRules: '',
        owner: '',
        isApiAccess: false,
        studentTokenLimit: 0,
        employeeTokenLimit: 0,
      };
      this.loadAll();
    });
  }

  editService(service: ServiceCatalogItem): void {
    this.serviceEdit = { ...service };
  }

  saveService(): void {
    if (!this.serviceEdit) {
      return;
    }
    this.http
      .put<ServiceCatalogItem>(`${this.baseUrl}/services/${this.serviceEdit.id}`, this.serviceEdit)
      .subscribe(() => {
        this.serviceEdit = null;
        this.loadAll();
      });
  }

  deleteService(serviceId: number): void {
    this.http.delete(`${this.baseUrl}/services/${serviceId}`).subscribe(() => this.loadAll());
  }

  submitPolicy(): void {
    if (!this.policyDraft.title) {
      return;
    }
    this.http.post<PolicyDocument>(`${this.baseUrl}/policies`, this.policyDraft).subscribe(() => {
      this.policyDraft = {
        id: 0,
        title: '',
        version: '',
        summary: '',
        publishedOn: new Date().toISOString(),
      };
      this.loadAll();
    });
  }

  editPolicy(policy: PolicyDocument): void {
    this.policyEdit = { ...policy };
  }

  savePolicy(): void {
    if (!this.policyEdit) {
      return;
    }
    this.http
      .put<PolicyDocument>(`${this.baseUrl}/policies/${this.policyEdit.id}`, this.policyEdit)
      .subscribe(() => {
        this.policyEdit = null;
        this.loadAll();
      });
  }

  deletePolicy(policyId: number): void {
    this.http.delete(`${this.baseUrl}/policies/${policyId}`).subscribe(() => this.loadAll());
  }

  submitRole(): void {
    if (!this.roleDraft.name) {
      return;
    }
    this.http.post<RoleProfile>(`${this.baseUrl}/roles`, this.roleDraft).subscribe(() => {
      this.roleDraft = { id: 0, name: '', responsibilities: '' };
      this.loadAll();
    });
  }

  editRole(role: RoleProfile): void {
    this.roleEdit = { ...role };
  }

  saveRole(): void {
    if (!this.roleEdit) {
      return;
    }
    this.http.put<RoleProfile>(`${this.baseUrl}/roles/${this.roleEdit.id}`, this.roleEdit).subscribe(() => {
      this.roleEdit = null;
      this.loadAll();
    });
  }

  deleteRole(roleId: number): void {
    this.http.delete(`${this.baseUrl}/roles/${roleId}`).subscribe(() => this.loadAll());
  }

  submitPrompt(): void {
    if (!this.promptDraft.title) {
      return;
    }
    this.http.post<PromptLibraryItem>(`${this.baseUrl}/prompts`, this.promptDraft).subscribe(() => {
      this.promptDraft = { id: 0, track: '', title: '', description: '', status: '' };
      this.loadAll();
    });
  }

  editPrompt(prompt: PromptLibraryItem): void {
    this.promptEdit = { ...prompt };
  }

  savePrompt(): void {
    if (!this.promptEdit) {
      return;
    }
    this.http
      .put<PromptLibraryItem>(`${this.baseUrl}/prompts/${this.promptEdit.id}`, this.promptEdit)
      .subscribe(() => {
        this.promptEdit = null;
        this.loadAll();
      });
  }

  deletePrompt(promptId: number): void {
    this.http.delete(`${this.baseUrl}/prompts/${promptId}`).subscribe(() => this.loadAll());
  }
}
