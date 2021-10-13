Feature: Healing
Characters are able to heal themselves and allies, as long as these characters are alive. Healing a character increases
their Health, but can never increase it above the maximum of 1000.

    Scenario: Characters can heal themselves
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed | 500    |
        When "Hector The Well-Endowed" heals themselves by 100
        Then the health of "Hector The Well-Endowed" should be 600

    Scenario: Characters cannot heal other characters
        Given the following characters:
          | Name                    | Health |
          | Hector The Well-Endowed | 500    |
          | Duquesne                | 1000   |
        Then "Duquesne" should not be able to heal "Hector The Well-Endowed"

    Scenario: Dead characters cannot be healed by anyone
        Given the following characters:
          | Name                   | Health |
          | Zippadeedoo            | 0      |
          | Marrrrr                | 1000   |
          | Naked Pierce Hawthorne | 1000   |
        And "Zippadeedoo" and "Marrrrr" are allies
        Then "Zippadeedoo" should not be able to heal "Zippadeedoo"
        And "Marrrrr" should not be able to heal "Zippadeedoo"
        And "Naked Pierce Hawthorne" should not be able to heal "Zippadeedoo"

    Scenario: Healing negative health is not possible
        Given the following characters:
          | Name      | Health |
          | Lavernica | 500    |
        When "Lavernica" heals themselves by -100
        Then the health of "Lavernica" should be 500

    Scenario: Characters cannot be healed above their maximum health
        Given the following characters:
          | Name     | Health |
          | Duquesne | 500    |
        When "Duquesne" heals themselves by 600
        Then the health of "Duquesne" should be 1000