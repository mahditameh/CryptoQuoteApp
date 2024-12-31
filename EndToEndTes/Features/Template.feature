Feature: Conference configuration scenarios for creating and editing Conference settings
In order to create or update a Conference configuration
As a Business Customer
I want to be able to create or update a Conference and set its properties
Background:
Given the Business Customer selected the Create Conference option
Scenario: An existing unpublished Conference is selected and published
Given this conference information
| Owner | Email | Name | Description | Slug | Start | End |
| William Flash | william@fabrikam.com | CQRS2012P | CQRS summit 2012 conference (Published)| random | 05/02/2012 | 05/12/2012 |
And the Business Customer proceeds to create the Conference
When the Business Customer proceeds to publish the Conference
Then the state of the Conference changes to Published
Scenario: An existing Conference is edited and updated
Given an existing published conference with this information
| Owner | Email | Name | Description | Slug | Start | End |
| William Flash | william@fabrikam.com | CQRS2012U | CQRS summit 2012 conference (Original) | random | 05/02/2012 | 05/12/2012 |
And the Business Customer proceeds to edit the existing settings with this information
| Description |
| CQRS summit 2012 conference (Updated)|
When the Business Customer proceeds to save the changes
Then this information appears in the Conference settings
| Description |

