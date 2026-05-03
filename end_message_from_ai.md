### Iteration 1

I have implemented the Iteration 1 backend prototype following the strict guidelines laid out in your specification and the agent persona profile.

Here is a summary of the implementation:

1. Architectural Integrity & Internal Data Model
SubtitleQc.Core Library (.NET 10): Created the root C# project, ensuring zero external dependencies (purely standard System libraries).
Internal Agnostic Data Model: Added the Cue model to represent an abstracted, format-agnostic subtitle unit, along with QcResult and QcReport models. All data representations are straightforward objects that can natively serialize to JSON.
Parsers Module: Built basic SrtParser and WebVttParser implementations complying with IParser. They safely decouple reading string inputs from the validation rules by projecting the data solely into the internal Cue data model.
2. QC Rule Engine (Clean Code & SRP)
Dependency Inversion: Constructed an IQcRule interface that acts as the contract for all rules.
Rule Engine: Evaluates a stream of generic Cues against a list of injected rules.
Rules Implemented:
MaxLinesRule
MaxCplRule (Characters Per Line)
MaxCpsRule (Characters Per Second)
MinDurationRule
OverlapCheckRule
EmptyCueCheckRule
Each rule lives in its own file, fulfilling the Single Responsibility Principle, and no method exceeds 20 lines of code.
3. Test Integrity & Telemetry Protocol
I encountered broken dependencies in the existing SubtitleQc.Tests suite, so I fixed the csproj missing test package versions, without modifying the immutable objective test scenarios.
All 13 tests pass successfully (dotnet test returned a 100% pass rate on the second attempt).
Logging was rigorously maintained, generating a test_metrics.jsonl file in the root structure tracking both the failing build baseline and the finalized, successful validation telemetry matching your exact JSON layout.
The engine is now fully functional, testable, and prepared for future specification iterations!


### Iteration 2

I have completed Iteration 2 by expanding the system to support the TTML (XML-based) subtitle format.

Summary of Implementation (Iteration 2)
Parsers Expansion (TtmlParser.cs):

Added TtmlParser which implements the IParser interface.
Designed it using System.Xml.Linq to properly handle XML nodes (<tt>, <body>, <div>, <p>, <span>, <br>).
Converts the <p> timing (begin and end attributes) and text nodes directly into the universal Cue data model so that no existing rules have to be modified.
Preservation of Logic:

As per the requirements, the core RuleEngine and all validation logic (MaxLinesRule, MaxCplRule, etc.) remained entirely untouched.
All 13 existing Unit Tests passed successfully on the first test execution since the underlying Cue model projection ensures backward compatibility and architectural integrity.
Telemetry:

I logged the dotnet test execution to test_metrics.jsonl under project_iteration 2, preserving the exact JSON schema required by the specification. The run achieved a 100% pass rate.
The system is now ready for the next iteration! Let me know when you'd like to proceed to the next iteration!


### Iteration 3

I have implemented Iteration 3 by expanding the system to handle external visual cut (shot change) data and integrating new QC rules that utilize this data.

Summary of Implementation (Iteration 3)
Internal Data Model Upgrade (Cue.cs):

Safely extended the unified Cue data model by adding an optional StartFrame property to hold frame-specific attributes.
Updated the constructor to handle this optional parameter, preserving full backwards compatibility with Iteration 1 and 2 parsers.
External Data Provider Abstraction (IShotChangeProvider.cs):

Introduced a new interface IShotChangeProvider to represent the external shot change data.
The interface provides GetShotChangeTimestamps() and GetShotChangeFrames() ensuring the rule engine isn't directly coupled to the data source implementations.
Rule Engine Expansion:

CrossShotBoundaryCheckRule: Added a new rule that flags a Cue if it spans across a cut timestamp retrieved from the IShotChangeProvider.
MinFramesFromShotChangeRule: Added a new rule ensuring that a cue does not begin too close (within a given frame threshold) to a cut boundary based on frame values.
Telemetry & Tests:

Executed dotnet test, running all 19 tests across all iterations (including 6 new Gherkin-based shot change tests from Iteration3ShotChangeRulesTests.cs).
All tests passed successfully on the first run (100% pass rate).
Logged the results to test_metrics.jsonl matching the required telemetry schema for Iteration 3.
The system perfectly adheres to the Single Responsibility Principle and the Open/Closed Principle. The core logic handles the new external properties without altering previous rules. Let me know if there's anything else!