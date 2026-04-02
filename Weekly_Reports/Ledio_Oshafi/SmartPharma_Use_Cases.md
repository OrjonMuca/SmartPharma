# 📋 SmartPharma — Use Case Specification

> A complete specification of all system use cases for the SmartPharma platform, organized by actor role.

---
## 👤 Administrator Use Cases

### UC01 — Manage User Accounts

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | None |
| **Summary** | Administrator creates, updates, or deletes any user account on the platform. |
| **Preconditions** | Administrator is logged in to the SmartPharma system. |
| **Postconditions** | User account records are updated, created, or removed in the system database. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to the User Management section. |
| 2 | System displays a list of all registered users. |
| 3 | Administrator selects an action: create, update, or delete. |
| 4 | Administrator fills in or modifies the required user data. |
| 5 | System validates the data and saves changes. |
| 6 | System confirms the action with a success message. |

####  Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator attempts to delete a user with active dependencies. |
| 2 | System displays a warning and prevents deletion. |
| 3 | Administrator resolves dependencies or cancels the action. |

#### Non-Functional Requirements

- Changes must be saved within **2 seconds**.
- All actions must be logged in the **audit trail** for security compliance.

---

### UC02 — Manage User Roles

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | [UC01 — Manage User Accounts](#uc01--manage-user-accounts) |
| **Summary** | Administrator assigns, modifies, or revokes roles for any platform user. |
| **Preconditions** | Administrator is logged in; target user account exists in the system. |
| **Postconditions** | User role is updated and new access permissions are enforced across the platform. |

####  Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to User Management and selects a user. |
| 2 | System displays the current role assigned to the user. |
| 3 | Administrator selects a new role from available options: Customer, Pharmacist, or Administrator. |
| 4 | System validates the role change. |
| 5 | System applies the updated role and adjusts permissions immediately. |
| 6 | System notifies the affected user of the role change. |

####  Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator attempts to remove the last Administrator account. |
| 2 | System displays an error preventing the action. |
| 3 | Administrator assigns a new Administrator before revoking the existing one. |

#### Non-Functional Requirements

- Role changes must take effect **immediately** upon saving.
- Role assignment must follow **least-privilege** security principles.

---

### UC03 — Manage Pharmacist Accounts

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | [UC01](#uc01--manage-user-accounts), [UC02](#uc02--manage-user-roles) |
| **Summary** | Administrator reviews, approves, suspends, or removes pharmacist accounts. |
| **Preconditions** | Administrator is logged in; one or more pharmacist account requests or records exist. |
| **Postconditions** | Pharmacist account status is updated; access to the platform is granted or revoked accordingly. |

####  Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to the Pharmacist Accounts section. |
| 2 | System displays a list of pharmacist accounts and their statuses. |
| 3 | Administrator selects an account to review. |
| 4 | Administrator verifies credentials and professional information. |
| 5 | Administrator approves, suspends, or removes the account. |
| 6 | System updates account status and notifies the pharmacist. |

####  Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator cannot verify submitted credentials. |
| 2 | Administrator flags the account as pending and requests additional documents. |
| 3 | Pharmacist resubmits credentials for further review. |

#### Non-Functional Requirements

- Pharmacist credential data must be **stored securely and encrypted at rest**.
- Status changes must be logged with **timestamp and administrator ID**.

---

### UC04 — Monitor Platform Activity

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | None |
| **Summary** | Administrator views logs of user actions, system events, and usage statistics. |
| **Preconditions** | Administrator is logged in; system activity logs are available. |
| **Postconditions** | Activity report is displayed; no system state is modified. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to the Activity Monitor dashboard. |
| 2 | System displays real-time and historical activity logs. |
| 3 | Administrator applies filters by date range, user, or event type. |
| 4 | System returns filtered results. |
| 5 | Administrator reviews the activity data. |
| 6 | Administrator optionally exports the report. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | No activity records match the applied filters. |
| 2 | System displays an empty state message. |
| 3 | Administrator adjusts filter criteria and retries. |

#### Non-Functional Requirements

- Activity logs must be retained for a minimum of **90 days**.
- Dashboard must load within **3 seconds** for datasets up to 100,000 records.

---

### UC05 — Oversee Medicine Listings

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | [UC09](#uc09--add-medicine-to-catalog), [UC10](#uc10--update-medicine-information) |
| **Summary** | Administrator reviews, approves, flags, or removes any medicine listing on the platform. |
| **Preconditions** | Administrator is logged in; at least one medicine listing exists in the system. |
| **Postconditions** | Medicine listing status is updated or the listing is removed from the public catalog. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to the Medicine Listings section. |
| 2 | System displays all listings with status indicators. |
| 3 | Administrator selects a listing to review. |
| 4 | System displays full listing details. |
| 5 | Administrator approves, flags for review, or removes the listing. |
| 6 | System updates listing status and notifies the responsible pharmacist. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator flags a listing for compliance review. |
| 2 | System temporarily hides the listing from customers. |
| 3 | Pharmacist is notified and must update the listing before resubmission. |

#### Non-Functional Requirements

- Listing status changes must be reflected on the customer-facing catalog within **60 seconds**.
- All moderation actions must be **auditable**.

---

### UC06 — Maintain Access Control

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | [UC02 — Manage User Roles](#uc02--manage-user-roles) |
| **Summary** | Administrator configures role-based permissions and platform security policies. |
| **Preconditions** | Administrator is logged in with full system privileges. |
| **Postconditions** | Access control rules are updated and enforced across all user sessions on the platform. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to Access Control settings. |
| 2 | System displays the current permission matrix by role. |
| 3 | Administrator modifies permissions for a selected role. |
| 4 | System validates the configuration for conflicts. |
| 5 | Administrator saves the updated access policy. |
| 6 | System applies new permissions platform-wide immediately. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator configures a permission that creates a conflict or security gap. |
| 2 | System highlights the conflict and blocks saving. |
| 3 | Administrator revises the configuration to resolve the conflict. |

#### Non-Functional Requirements

- Permission changes must propagate to all active sessions within **5 seconds**.
- Configuration changes must require **two-factor authentication** confirmation.

---

### UC07 — Manage System Data

| Field | Details |
|---|---|
| **Actor** | Administrator |
| **Dependency** | None |
| **Summary** | Administrator maintains platform-wide data including categories, configurations, and core records. |
| **Preconditions** | Administrator is logged in with full data management privileges. |
| **Postconditions** | System data is updated and all linked modules reflect the changes. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator navigates to System Data management. |
| 2 | System displays all configurable data entities (categories, settings, reference data). |
| 3 | Administrator selects a data entity to manage. |
| 4 | Administrator creates, updates, or deletes records. |
| 5 | System validates the changes. |
| 6 | System saves changes and confirms success. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Administrator attempts to delete a data record referenced by existing medicines or users. |
| 2 | System prevents deletion and displays linked dependencies. |
| 3 | Administrator resolves dependencies before retrying. |

#### Non-Functional Requirements

- System data changes must be **backed up automatically** before modification.
- All changes must be **reversible within a 24-hour window**.

---

## Pharmacist Use Cases

### UC08 — Login / Authenticate (Pharmacist)

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC03 — Manage Pharmacist Accounts](#uc03--manage-pharmacist-accounts) |
| **Summary** | Pharmacist logs in to the platform to access inventory and listing management functions. |
| **Preconditions** | Pharmacist holds an approved and active account on SmartPharma. |
| **Postconditions** | Pharmacist is authenticated and an active session is established; Pharmacist dashboard is accessible. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to the SmartPharma login page. |
| 2 | Pharmacist enters email and password credentials. |
| 3 | System validates credentials against the user database. |
| 4 | System verifies that the account has the Pharmacist role and is active. |
| 5 | System creates an authenticated session. |
| 6 | Pharmacist is redirected to the Pharmacist dashboard. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist enters incorrect credentials. |
| 2 | System displays an error message without revealing which field is wrong. |
| 3 | After 5 failed attempts, system temporarily locks the account and notifies the administrator. |

#### Non-Functional Requirements

- Authentication must complete within **2 seconds**.
- Sessions must expire after **30 minutes** of inactivity.
- Passwords must be stored using a **secure hashing algorithm**.

---

### UC09 — Add Medicine to Catalog

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC08 — Login / Authenticate (Pharmacist)](#uc08--login--authenticate-pharmacist) |
| **Summary** | Pharmacist creates a new medicine listing with full product details in the SmartPharma catalog. |
| **Preconditions** | Pharmacist is logged in; medicine does not already exist in the catalog. |
| **Postconditions** | A new medicine entry is created in the catalog and visible to customers browsing the platform. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to Add Medicine in the dashboard. |
| 2 | System presents the medicine entry form. |
| 3 | Pharmacist enters name, dosage, form, category, description, and availability. |
| 4 | Pharmacist submits the form. |
| 5 | System validates all required fields. |
| 6 | System saves the listing and makes it visible in the customer catalog. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist submits the form with missing required fields. |
| 2 | System highlights the missing fields and displays validation errors. |
| 3 | Pharmacist corrects the errors and resubmits. |

#### Non-Functional Requirements

- New listings must appear in the customer-facing catalog within **60 seconds** of saving.
- The form must support **image uploads up to 5 MB**.

---

### UC10 — Update Medicine Information

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC08](#uc08--login--authenticate-pharmacist), [UC09](#uc09--add-medicine-to-catalog) |
| **Summary** | Pharmacist edits the details of an existing medicine record in the catalog. |
| **Preconditions** | Pharmacist is logged in; the target medicine record exists and belongs to this pharmacist. |
| **Postconditions** | Medicine record is updated with the new information and the catalog reflects the latest details. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to Medicine Records and selects a listing. |
| 2 | System displays the current medicine details in an editable form. |
| 3 | Pharmacist modifies the relevant fields (e.g. description, dosage, price). |
| 4 | Pharmacist saves the changes. |
| 5 | System validates the updated data. |
| 6 | System saves the changes and updates the listing in the catalog. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist attempts to edit a listing locked by the Administrator for review. |
| 2 | System displays a notice that the listing is under moderation. |
| 3 | Pharmacist contacts the Administrator to resolve the lock. |

#### Non-Functional Requirements

- **Edit history** must be maintained so previous versions can be reviewed.
- Changes must be reflected in the live catalog within **60 seconds**.

---

### UC11 — Manage Product Availability

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC08](#uc08--login--authenticate-pharmacist), [UC09](#uc09--add-medicine-to-catalog) |
| **Summary** | Pharmacist updates the availability status of a medicine (available, out of stock, discontinued). |
| **Preconditions** | Pharmacist is logged in; the target medicine record exists. |
| **Postconditions** | Medicine availability status is updated and accurately reflected to customers on the platform. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to the medicine listing. |
| 2 | System displays the current availability status. |
| 3 | Pharmacist selects a new availability status. |
| 4 | System validates the status change. |
| 5 | System saves the updated status. |
| 6 | Customer-facing listing is updated to reflect the new availability. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist marks a medicine as discontinued. |
| 2 | System prompts for confirmation since this action hides the listing from customers. |
| 3 | Pharmacist confirms and system archives the listing. |

#### Non-Functional Requirements

- Availability changes must be reflected on the customer catalog within **30 seconds**.
- Out-of-stock items must be **visually differentiated** but remain searchable.

---

### UC12 — Manage Pharmaceutical Listings

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC08](#uc08--login--authenticate-pharmacist), [UC09](#uc09--add-medicine-to-catalog), [UC10](#uc10--update-medicine-information), [UC11](#uc11--manage-product-availability) |
| **Summary** | Pharmacist organises and maintains the full set of medicine listings under their account. |
| **Preconditions** | Pharmacist is logged in; at least one medicine listing exists under their account. |
| **Postconditions** | All listings under the pharmacist's account are organised, accurate, and up to date in the catalog. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to My Listings in the dashboard. |
| 2 | System displays all medicines associated with the pharmacist's account. |
| 3 | Pharmacist applies sorting or filtering (by name, category, availability). |
| 4 | Pharmacist selects a listing to manage. |
| 5 | Pharmacist performs an action: edit, update availability, or archive. |
| 6 | System saves changes and updates the catalog. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist searches for a listing that does not exist in their account. |
| 2 | System returns no results. |
| 3 | Pharmacist navigates to Add Medicine to create the missing listing. |

#### Non-Functional Requirements

- Interface must support **pagination** for accounts with more than 100 medicines.
- Search and filter operations must return results within **1 second**.

---

### UC13 — View Medicine Records

| Field | Details |
|---|---|
| **Actor** | Pharmacist |
| **Dependency** | [UC08 — Login / Authenticate (Pharmacist)](#uc08--login--authenticate-pharmacist) |
| **Summary** | Pharmacist browses and searches through all medicine records they manage. |
| **Preconditions** | Pharmacist is logged in; medicine records exist under their account. |
| **Postconditions** | Medicine records are displayed to the Pharmacist; no data is modified. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist navigates to the Medicine Records section. |
| 2 | System displays the full list of medicines associated with the pharmacist. |
| 3 | Pharmacist uses search or filter to find specific records. |
| 4 | System returns matching results. |
| 5 | Pharmacist selects a record to view its full details. |
| 6 | System displays the medicine detail page. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Pharmacist searches with a term that returns no matches. |
| 2 | System displays an empty results message. |
| 3 | Pharmacist adjusts the search term or clears filters to browse the full list. |

#### Non-Functional Requirements

- Record list must load within **2 seconds**.
- Search must be **case-insensitive** and support **partial name matching**.

---

## Customer Use Cases

### UC14 — Create Personal Account

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | None |
| **Summary** | A new user registers a personal customer account on the SmartPharma platform. |
| **Preconditions** | The user does not already have a registered account on SmartPharma. |
| **Postconditions** | A new Customer account is created and active; user can immediately log in and browse the platform. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | User navigates to the registration page. |
| 2 | System displays the account creation form. |
| 3 | User enters name, email address, and password. |
| 4 | System validates the input (unique email, password strength). |
| 5 | System creates the account and assigns the Customer role. |
| 6 | System sends a confirmation email and redirects user to the dashboard. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | User enters an email address already registered in the system. |
| 2 | System displays an error indicating the email is in use. |
| 3 | User is offered the option to log in or reset their password instead. |

#### Non-Functional Requirements

- Registration must complete within **3 seconds**.
- Passwords must meet **minimum strength requirements**.
- Confirmation email must be sent within **60 seconds**.

---

### UC15 — Login / Authenticate (Customer)

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | [UC14 — Create Personal Account](#uc14--create-personal-account) |
| **Summary** | Customer logs in to the platform using registered credentials to access personal account features. |
| **Preconditions** | Customer has a registered and active account on SmartPharma. |
| **Postconditions** | Customer is authenticated and a session is started; the personal dashboard is accessible. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer navigates to the login page. |
| 2 | Customer enters email and password. |
| 3 | System validates the credentials. |
| 4 | System verifies the account is active and has the Customer role. |
| 5 | System creates an authenticated session. |
| 6 | Customer is redirected to their personal dashboard. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Customer enters incorrect credentials. |
| 2 | System displays a generic error without specifying which field is wrong. |
| 3 | After 5 failed attempts, the account is temporarily locked and the customer is advised to reset their password. |

#### Non-Functional Requirements

- Authentication must complete within **2 seconds**.
- Sessions must expire after **60 minutes** of inactivity.
- Login page must be served over **HTTPS**.

---

### UC16 — Manage Personal Account

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | [UC15](#uc15--login--authenticate-customer) — Includes [UC17](#uc17--update-account-information) |
| **Summary** | Customer views and updates their personal profile on SmartPharma. |
| **Preconditions** | Customer is logged in to their account. |
| **Postconditions** | Customer profile information is updated and saved in the system. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer navigates to Account Settings. |
| 2 | System displays current profile information. |
| 3 | Customer selects the field(s) to update. |
| 4 | UC17 (Update account information) is invoked. |
| 5 | System validates and saves the changes. |
| 6 | System confirms the update with a success notification. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Customer attempts to change email to one already registered by another user. |
| 2 | System rejects the change and notifies the customer. |
| 3 | Customer enters a different email address. |

#### Non-Functional Requirements

- Profile updates must be saved **immediately** and reflected across all sessions.
- Account data must be **encrypted in transit and at rest**.

---

### UC17 — Update Account Information

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | [UC15](#uc15--login--authenticate-customer) — Included by [UC16](#uc16--manage-personal-account) |
| **Summary** | Customer changes their name, email address, or password on their SmartPharma profile. |
| **Preconditions** | Customer is logged in; customer accesses Account Settings. |
| **Postconditions** | Account details are saved successfully and the updated information is active immediately. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer selects the field to update (name, email, or password). |
| 2 | System displays the relevant update form. |
| 3 | Customer enters the new value and confirms (password requires current password confirmation). |
| 4 | System validates the new data. |
| 5 | System saves the updated information. |
| 6 | System confirms the change and, if email was changed, sends a verification email. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Customer enters an incorrect current password when trying to change their password. |
| 2 | System rejects the request and displays an error. |
| 3 | Customer is offered the option to reset their password via email. |

#### Non-Functional Requirements

- Password changes must **invalidate all other active sessions** for security.
- Email change must trigger a **re-verification flow**.

---

### UC18 — Browse Available Medicines

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | None *(login not required)*. Extends [UC19](#uc19--filter-by-category), [UC22](#uc22--view-medicine-details) |
| **Summary** | Customer explores the SmartPharma medicine catalog to discover available products. |
| **Preconditions** | At least one medicine listing is available and active in the catalog. |
| **Postconditions** | Medicine catalog is displayed to the customer; no system state is changed. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer navigates to the Medicine Catalog. |
| 2 | System loads and displays all available medicine listings. |
| 3 | Customer scrolls through the catalog. |
| 4 | Customer selects a medicine to view its details (UC22 is triggered). |
| 5 | Customer returns to the catalog to continue browsing. |
| 6 | Customer optionally applies filters (UC19 is triggered). |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | The catalog contains no active listings. |
| 2 | System displays an empty state message. |
| 3 | Customer is prompted to check back later or contact a pharmacist. |

#### Non-Functional Requirements

- Catalog page must load within **2 seconds**.
- Listings must be **paginated at 20 items** per page.
- Catalog must be **accessible without login**.

---

### UC19 — Filter by Category

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | Extends [UC18 — Browse Available Medicines](#uc18--browse-available-medicines) |
| **Summary** | Customer narrows the medicine catalog by therapeutic category or availability status. |
| **Preconditions** | Customer is browsing the medicine catalog; filter options are available. |
| **Postconditions** | A filtered subset of the medicine catalog is displayed to the customer. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer selects a filter option (category, availability, or form). |
| 2 | System applies the selected filter to the catalog. |
| 3 | System returns a filtered list of matching medicines. |
| 4 | Customer reviews the filtered results. |
| 5 | Customer selects a medicine or adjusts the filter further. |
| 6 | Customer clears filters to return to the full catalog. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | The applied filter returns no matching medicines. |
| 2 | System displays an empty results state with a suggestion to broaden the filter. |
| 3 | Customer clears or changes the filter criteria. |

#### Non-Functional Requirements

- Filter operations must return results within **1 second**.
- Multiple filters must be **combinable** without performance degradation.

---

### UC20 — Search for Pharmaceutical Products

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | Extends [UC21 — View Search Results](#uc21--view-search-results) |
| **Summary** | Customer enters a search term to find specific medicines by name or keyword. |
| **Preconditions** | Customer is on the SmartPharma platform; search functionality is available. |
| **Postconditions** | Matching medicines are listed for the customer; no system state is changed. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer enters a search term in the search bar. |
| 2 | System processes the search query against the medicine catalog. |
| 3 | System returns a list of matching results (UC21 is triggered). |
| 4 | Customer reviews the results. |
| 5 | Customer selects a medicine to view its details. |
| 6 | Customer refines the search term if needed. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | The search term returns no matching medicines. |
| 2 | System displays a no-results message with suggested alternatives or related terms. |
| 3 | Customer adjusts the search term and retries. |

#### Non-Functional Requirements

- Search results must be returned within **1 second**.
- Search must support **partial name matching** and be **case-insensitive**.
- Search must work **without requiring login**.

---

### UC21 — View Search Results

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | Depends on [UC20](#uc20--search-for-pharmaceutical-products). Extended by [UC20](#uc20--search-for-pharmaceutical-products). |
| **Summary** | Customer reviews the list of medicines returned by a search query. |
| **Preconditions** | Customer has submitted a search term; the system has processed the query. |
| **Postconditions** | Search results are displayed; customer can select any item to proceed to the detail view. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | System displays the search results page with matched medicines. |
| 2 | Each result shows the medicine name, dosage, form, and availability. |
| 3 | Customer scrolls through the results. |
| 4 | Customer selects a result to view full medicine details. |
| 5 | System navigates to the medicine detail page (UC22). |
| 6 | Customer returns to results to continue reviewing. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Search returns a very large number of results. |
| 2 | System paginates results and prompts the customer to refine their search. |
| 3 | Customer uses filters (UC19) to narrow down the results. |

#### Non-Functional Requirements

- Results must be **sorted by relevance** by default.
- Pagination must be applied at **20 results per page**.
- Results page must render within **1 second**.

---

### UC22 — View Medicine Details

| Field | Details |
|---|---|
| **Actor** | Customer |
| **Dependency** | Extended by [UC18](#uc18--browse-available-medicines), [UC21](#uc21--view-search-results) |
| **Summary** | Customer reads the full description, dosage information, and availability of a specific medicine. |
| **Preconditions** | Customer has selected a medicine from the catalog or search results; the medicine listing is active. |
| **Postconditions** | Full medicine detail is displayed to the customer; no system state is changed. |

#### Main Sequence

| Step | Description |
|------|-------------|
| 1 | Customer selects a medicine from the catalog or search results. |
| 2 | System loads the medicine detail page. |
| 3 | System displays name, dosage, form, category, full description, instructions, and availability. |
| 4 | Customer reads the medicine information. |
| 5 | Customer navigates back to the catalog or search results. |
| 6 | Customer optionally contacts the pharmacist for further queries. |

#### Alternative Sequence

| Step | Description |
|------|-------------|
| 1 | Customer navigates to a medicine detail page that has since been removed or made unavailable. |
| 2 | System displays a not-found or unavailable message. |
| 3 | Customer is redirected back to the catalog. |

#### Non-Functional Requirements

- Medicine detail page must load within **2 seconds**.
- All pharmaceutical information displayed must reflect the **latest version** saved by the Pharmacist.
