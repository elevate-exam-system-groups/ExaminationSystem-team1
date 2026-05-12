# ExaminationSystem-team1
  
*Business Overview & Process Flow*

A complete guide to how the platform works,

who uses it, and the business rules that govern it.

*Version 1.0   |   April 2026*

| 1\. Executive Summary |
| :---- |

The Examination System is a digital learning and assessment platform that connects students with structured diploma programs through online quizzes. It allows educational institutions to publish learning tracks (Diplomas), attach timed assessments (Quizzes), and monitor student performance through rich analytics.

The platform serves two distinct user groups: Students who enroll in diploma programs and take quizzes to demonstrate mastery, and Administrators who manage all content, monitor progress, and make data-driven decisions.

| Students Learn at their own pace through structured diploma tracks and timed quizzes. | Admins Create, manage, and publish diplomas and quizzes; monitor all activity. | Platform Enforce business rules, secure assessments, and deliver analytics. |
| :---- | :---- | :---- |

| 2\. User Roles & Permissions |
| :---- |

The platform has two user roles. Each role has a clearly scoped set of actions they are allowed to perform.

## **2.1 Student**

A Student is any registered and email-verified user. Students can:

* Browse and view all published Diploma programs

* View the quizzes available inside each Diploma

* Start, answer, and submit Quiz attempts

* View their own quiz results and attempt history

* Track overall progress via their personal dashboard

| IMPORTANT | *Students can ONLY see their own data. They cannot view other students' attempts, scores, or personal information.* |
| :---: | :---- |

## **2.2 Administrator**

An Administrator has full control over the platform content and can monitor all activity. Admins can:

* Create, edit, and delete Diploma programs

* Create, edit, publish, and unpublish Quizzes

* Add, edit, and delete Questions and answer options

* View all student attempts and results across the platform

* Access aggregated analytics and performance reports

* Monitor the Admin Dashboard for platform-wide statistics

| NOTE | *Admins cannot perform student actions (e.g., take quizzes). Role separation is enforced at the system level via JWT claims.* |
| :---: | :---- |

| 3\. Core Content Model |
| :---- |

All learning content is organized in a hierarchy: Diplomas contain Quizzes, and Quizzes contain Questions. Understanding this hierarchy is essential to understanding every flow in the system.

| DIPLOMA  (e.g., "Web Development Fundamentals") ▼  Contains multiple QUIZZES  (e.g., "HTML & CSS Basics", "JavaScript Fundamentals") ▼  Each Quiz contains multiple QUESTIONS  (multiple-choice, exactly one correct answer each) |
| ----- |

## **3.1 Diploma**

| Field | Description |
| :---- | :---- |
| **Title** | Name of the diploma program (required, 3–200 characters) |
| **Description** | Optional overview of the diploma (max 1,000 characters) |
| **Status** | draft (not visible to students) or published (visible and accessible) |
| **Quiz Count** | Number of quizzes attached to this diploma |

## **3.2 Quiz**

| Field | Description |
| :---- | :---- |
| **Title** | Name of the quiz (required) |
| **Duration** | Time limit in minutes — enforced server-side |
| **Pass Score** | Minimum score % to pass (0–100, default: 60%) |
| **Max Attempts** | Maximum number of tries allowed per student (optional, default: unlimited) |
| **Status** | draft (cannot be taken) or published (available to students) |
| **Instructions** | Optional guidance shown to the student before starting |

## **3.3 Question**

| Field | Description |
| :---- | :---- |
| **Question Text** | The question prompt (required) |
| **Options** | Minimum 2 answer choices; exactly one must be marked correct |
| **Correct Answer** | Stored server-side — NEVER shown to students before submission |
| **Explanation** | Optional text shown to students after submitting, explaining the correct answer |
| **Order Index** | Controls display order (shuffled per attempt for students) |

| 4\. Student Business Flows |
| :---- |

## **4.1 Registration & Account Activation**

A new user must complete a two-step process before they can access the platform:

| 1 | Register The user provides their full name, email, and password. The system validates inputs, hashes the password, and creates an account in a pending state. A 6-digit One-Time Password (OTP) is sent to the provided email. |
| :---: | :---- |

| 2 | Verify Email (OTP) The user submits the 6-digit OTP received by email. The system validates it and activates the account. The OTP expires in 10 minutes and can be resent up to 3 times per hour. |
| :---: | :---- |

| 3 | Log In The user logs in with their credentials and receives a short-lived access token (15 minutes) and a long-lived refresh token (7 days). The account must be active and verified to log in. |
| :---: | :---- |

| RULE | *An account in 'pending' status cannot log in. Users must complete email verification first.* |
| :---: | :---- |

## **4.2 Password Recovery**

If a student forgets their password, a secure reset flow is available:

| 1 | Request Reset The student submits their email. The system always responds with a success message regardless of whether the email exists — this prevents attackers from discovering which emails are registered. |
| :---: | :---- |

| 2 | Click Reset Link If the email is registered, a secure single-use reset link (valid for 15 minutes) is sent. The link contains a token that is stored hashed in the database. |
| :---: | :---- |

| 3 | Set New Password The student submits their new password. The system validates the token, saves the new hashed password, and invalidates all existing sessions for security. |
| :---: | :---- |

## **4.3 Browsing the Learning Catalog**

After logging in, students can explore the available diploma programs:

* The Student Dashboard provides a snapshot of enrolled diplomas, recent quiz attempts, and overall statistics (average score, pass rate, time spent).

* Students can browse all published Diplomas. Each diploma shows its title, description, and the student's own progress (how many quizzes completed out of the total).

* Selecting a diploma lists all its published quizzes, showing each quiz's duration, pass threshold, attempt history, and whether the student can still attempt it.

| NOTE | *Students only see published content. Diplomas and Quizzes in 'draft' status are invisible to students.* |
| :---: | :---- |

## **4.4 Taking a Quiz — The Attempt Flow**

This is the core student experience. The flow is designed to be secure, fair, and tamper-resistant.

| 1 | Start Quiz The student initiates a quiz. The system checks: is the quiz published? Has the student exhausted their attempts? If an in-progress attempt already exists, the student is directed to resume it instead of creating a duplicate. If all checks pass, a new Attempt record is created and the student receives the questions — with both question order and answer option order shuffled uniquely for their attempt. |
| :---: | :---- |

| 2 | Answer Questions The student answers questions one at a time. Each answer is saved immediately to the server. Re-answering a question overwrites the previous answer. The server validates the deadline on every answer submission — if time has run out, the attempt is auto-submitted at that moment. |
| :---: | :---- |

| 3 | Monitor Time The quiz has a strict server-enforced time limit. The student can check remaining time at any point; the server calculates this as (deadline minus current server time). Client-side timers are cosmetic only and cannot be manipulated to extend time. |
| :---: | :---- |

| 4 | Submit Quiz The student explicitly submits, or the timer expires. The system calculates the score: (correct answers / total questions) x 100\. If the score meets or exceeds the pass threshold, the attempt is marked as passed. |
| :---: | :---- |

| 5 | View Results After submission, the student can view their full result breakdown: their chosen answer for each question, whether it was correct, the correct answer, and an explanation. This information is only available after the quiz is submitted — never during an active attempt. |
| :---: | :---- |

| Submitted Student clicked Submit before the deadline. | Timed Out Timer expired before student submitted. Score is based only on answers received before the deadline. | In Progress Quiz is ongoing. Results and correct answers are hidden. |
| :---- | :---- | :---- |

| 5\. Administrator Business Flows |
| :---- |

## **5.1 Creating & Publishing a Diploma Program**

A Diploma program must be fully built and published before students can see or access it. Publishing is a deliberate, gated action.

| 1 | Create Diploma Admin creates a diploma with a title and optional description. Status is automatically set to 'draft'. No student can see it yet. |
| :---: | :---- |

| 2 | Create Quizzes Admin adds one or more quizzes to the diploma, specifying the title, duration, pass score, max attempts, and instructions. Each quiz also starts in 'draft' status. |
| :---: | :---- |

| 3 | Add Questions For each quiz, Admin adds questions with at least 2 answer options. Exactly one option must be marked as correct. An optional explanation can be added to guide students after submission. |
| :---: | :---- |

| 4 | Validate Quiz Before publishing, Admin can run a readiness check that returns a pass/fail checklist (e.g., has at least 1 question, all questions have a correct answer marked, valid duration and pass score). |
| :---: | :---- |

| 5 | Publish Quiz Once all checks pass, Admin publishes each quiz individually. A published quiz becomes available to students. The diploma becomes visible once at least one of its quizzes is published. |
| :---: | :---- |

| RULE | *A quiz CANNOT be published if it has no questions, or if any question has no correct answer marked. The system enforces this and returns a detailed error checklist.* |
| :---: | :---- |

## **5.2 Editing Published Content**

Editing live content has specific constraints to protect ongoing student activity:

* Diploma title and description can be updated at any time via the update endpoint.

* Quiz settings (title, duration, pass score, instructions) can be updated at any time.

* Questions CANNOT be deleted from a published quiz. Admin must unpublish the quiz first.

* A quiz CANNOT be unpublished if any student currently has an in-progress attempt. Admin must wait for all active attempts to complete or time out.

* Status changes (publish/unpublish) are handled through dedicated actions, NOT through the edit endpoint.

## **5.3 Deleting Content**

The platform uses soft deletion — records are never permanently removed. Deletion rules:

| Content Type | Deletion Allowed? | Restriction |
| :---- | :---- | :---- |
| Diploma | Yes (soft delete) | Blocked if any student is actively enrolled |
| Quiz | Via unpublish then delete | Blocked while in-progress attempts exist |
| Question | Yes (soft delete) | Blocked while the quiz is in published status |

| 6\. Key Business Rules |
| :---- |

The following rules govern how the system behaves. They are enforced server-side and cannot be bypassed by clients.

## **6.1 Security Rules**

* Passwords are stored as bcrypt hashes with a minimum of 12 salt rounds. Plain-text passwords are never logged or stored.

* OTPs are stored hashed — only the hash is retained after the OTP is sent. The plain OTP exists only in the email.

* Password reset tokens are single-use and stored hashed. Using a token invalidates it immediately.

* Resetting a password revokes all existing sessions for that user account.

* After 5 consecutive failed login attempts, the account is locked for 15 minutes.

* OTPs are locked after 5 incorrect attempts. A new OTP must be requested.

| SECURITY | *Forgot Password always returns a success response regardless of whether the email is registered. This prevents email enumeration attacks.* |
| :---: | :---- |

## **6.2 Quiz & Attempt Rules**

* Questions and answer options are shuffled per attempt server-side. Two students taking the same quiz will see a different order.

* Correct answers are NEVER exposed during an active attempt — only after submission.

* A student can only have ONE in-progress attempt for a given quiz at a time.

* If a student tries to start a quiz with an existing in-progress attempt, they are returned the existing attempt ID to resume.

* The server is the single source of truth for time. Client-side timers cannot be manipulated to extend deadlines.

* Auto-submission is triggered on any request (answer, submit, or timer check) if the server detects the deadline has passed.

* Score formula: (Number of Correct Answers / Total Questions) x 100\. Passing requires score \>= quiz's pass threshold.

## **6.3 Content Lifecycle Rules**

* All newly created Diplomas and Quizzes start in 'draft' status.

* A quiz must pass all pre-publish checks before it can be published.

* Students only ever see 'published' content. Draft content is invisible to them.

* Diplomas and Questions use soft-delete only. Data is never permanently removed from the database.

* Max attempts per quiz is configurable. If not specified, students can attempt unlimited times.

## **6.4 Rate Limits**

To protect against abuse, authentication endpoints are rate-limited:

| Endpoint Group | Limit | Lockout |
| :---- | :---- | :---- |
| Register, Login, Verify OTP | 10 requests per minute | 429 Too Many Requests returned |
| All other authenticated endpoints | Standard (per IP) | Retry-After header returned |

| 7\. Analytics & Monitoring |
| :---- |

The platform provides administrators with visibility into student performance and platform health through two dashboards and a detailed analytics report.

## **7.1 Admin Dashboard**

A real-time snapshot of platform activity (cached for 5 minutes):

* Total registered users

* Active users today (derived from login/session logs)

* Total diploma programs and quizzes

* Total quiz attempts across all students

* Overall average pass rate

## **7.2 Performance Analytics**

Filterable by date range and diploma, providing deeper insight into assessment quality:

| Metric | What It Shows |
| :---- | :---- |
| **Pass Rate by Quiz** | For each quiz: how many students passed vs. total attempts. Identifies easy or overly difficult assessments. |
| **Avg Score by Diploma** | Average score across all quizzes within a diploma. Shows overall student competency per program. |
| **Attempts Over Time** | Daily attempt counts over the selected date range. Shows engagement trends. |
| **Top Failed Questions** | Questions where fewer than 40% of students answered correctly. Flags content that may be unclear, misleading, or poorly worded. |

| NOTE | *Analytics queries are cached for 10 minutes. When no data matches the selected filters, the response returns empty arrays and zeroes — never a 'not found' error.* |
| :---: | :---- |

## **7.3 Attempt Monitoring**

Admins can search and filter all student attempts across the platform:

* Filter by specific quiz, student, or attempt status (in-progress, submitted, timed-out)

* View full per-question breakdowns for any specific attempt

* Sort results by submission date in ascending or descending order

* All lists are paginated to maintain performance at scale

| 8\. End-to-End Scenario: A Student's Journey |
| :---- |

The following narrative walks through the complete experience of a student from registration to reviewing quiz results.

## **Step 1 — Sign Up**

Ahmed visits the platform and creates an account with his email and a strong password. His account is created in 'pending' status and the system sends him a 6-digit OTP via email.

## **Step 2 — Verify Email**

Ahmed opens the email, retrieves the OTP, and enters it on the platform. The system verifies it, activates his account, and he is now able to log in.

## **Step 3 — Log In**

Ahmed logs in. The system returns a JWT access token (valid 15 minutes) and a refresh token (valid 7 days). All subsequent requests include this token in the Authorization header.

## **Step 4 — Explore Diplomas**

Ahmed browses the catalog and finds the 'Web Development Fundamentals' diploma. He sees it has 8 quizzes. His progress shows 0 of 8 completed.

## **Step 5 — Start a Quiz**

Ahmed opens the 'HTML & CSS Basics' quiz (30 minutes, pass score 60%, max 3 attempts). He clicks Start. The server creates an attempt, records the start time, and returns 20 shuffled questions — without correct answers.

## **Step 6 — Answer & Submit**

Ahmed answers all 20 questions over 22 minutes. Each answer is saved to the server in real time. He submits with 8 minutes remaining. The server scores his answers: 17 correct out of 20 \= 85%. Since 85% \>= 60% pass threshold, he passes.

## **Step 7 — Review Results**

Ahmed views his full results: his answer for each question, whether it was right, the correct answer, and an explanation for each. He reviews the 3 questions he got wrong to learn from them.

## **Step 8 — Dashboard Update**

Ahmed's dashboard now reflects his completed attempt: 1 of 8 quizzes done in the diploma, overall stats updated, and the recent attempt appears in his history.

| 9\. Glossary |
| :---- |

| Term | Definition |
| :---- | :---- |
| **Diploma** | A structured learning program grouping multiple related quizzes under a single track. |
| **Quiz** | A timed, scored assessment containing multiple-choice questions. Part of a Diploma. |
| **Question** | A single multiple-choice item within a Quiz. Has 2–4 options; exactly one is correct. |
| **Attempt** | A single instance of a student taking a Quiz. Records answers, score, and timing. |
| **Draft** | Content status. Draft content is under construction and invisible to students. |
| **Published** | Content status. Published content is live and accessible to students. |
| **Pass Score** | The minimum percentage score required to pass a quiz. Configurable per quiz. |
| **Max Attempts** | The maximum number of times a student may take a specific quiz. |
| **OTP** | One-Time Password. A 6-digit code sent via email for account verification. Expires in 10 minutes. |
| **JWT** | JSON Web Token. A signed token that identifies the user and their role. Valid for 15 minutes. |
| **Refresh Token** | A long-lived token (7 days) used to get a new access token without re-logging in. |
| **Soft Delete** | Marking a record as deleted (with a timestamp) without removing it from the database. |
| **Auto-Submit** | Automatic quiz submission triggered when the server detects the time limit has expired. |
| **Timed Out** | Attempt status when auto-submitted by the server due to deadline expiration. |

