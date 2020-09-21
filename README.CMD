Instructions:

Build a component that will meet the following requirements. You may make assumptions if any requirements are ambiguous or vague but you must state the assumptions in your submission.

Overview:

You will be building a simple order calculator component that will provide tax and totals. The calculator will need to account for promotions, coupons, various tax rules, etc... You may assume that the database and data-access is already developed and may mock the data-access system. No UI elements will be built for this component.

Main Business Entities:

• Order: A set of products purchased by a customer.
• Product: A specific item a customer may purchase.
• Coupon: A discount for a specific product valid for a specified date range.
• Promotion: A business wide discount on all products valid for a specified date range.

Not all entities are necessarily listed – you may need to create additional models to complete the solution.

Business Rules:
• Tax is calculated as a simple percentage of the order total.
• Products categorized as ‘Luxury Items’ are taxed at twice the normal rate in certain states
• Tax is normally calculated after applying coupons and promotional discounts. However, in the following states, the tax must be calculated prior to applying the discount: FL, NM, and NV
• At this time, the business needs implementation for 5 clients that will use this application. 1 from GA, 1 from FL, 1 from NY, 1 from NM, and 1 from NV.

Technical Features:
As tax rules change frequently, your solution should be designed for maintainability.

Requirements:
• Adhering to the business rules stated previously:
o The application shall calculate the total cost of an order
o The application shall calculate the pre-tax cost of an order
o The application shall calculate the tax amount of an order
o The application shall store the final results in a repository upon completion of calculation.

Deliverables:
• .NET source code implementing all business rules and requirements
• Unit tests (you may choose the unit testing framework)
• A list of assumptions made during the implementation and a relative assessment of risk associated with those assumptions