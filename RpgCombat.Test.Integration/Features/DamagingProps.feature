Feature: Damaging Props

Props are non-character things that have a health and can be damaged by characters. Props do not deal damage and cannot
be healed. Once a prop's health drops to or below zero, it is Destroyed.

    Scenario: A prop with a health of or below zero is Destroyed
        Given the following props:
          | Name  | Health |
          | Chest | 0      |
          | Sword | -100   |
        Then the "Chest" should be Destroyed
        And the "Sword" should be Destroyed

    Scenario: Destroyed props cannot be damaged
        Given the following characters:
          | Name                   |
          | Naked Pierce Hawthorne |
        And the following props:
          | Name  | Health |
          | House | 0      |
        Then "Naked Pierce Hawthorne" should not be able to damage "House"

    Scenario: Dead characters cannot damage props
        Given the following characters:
          | Name     | Health |
          | Marrrrrr | 0      |
        And the following props:
          | Name | Health |
          | Cart | 500    |
        Then "Marrrrrr" should not be able to damage "Cart"

    Scenario: Damaging a prop reduces its Health
        Given the following characters:
          | Name                 |
          | Bing Bong The Archer |
        And the following props:
          | Name | Health |
          | Tree | 2000   |
        When "Bing Bong The Archer" attempts to deal 500 damage to "Tree"
        Then the health of "Tree" should be 1500
        When "Bing Bong The Archer" attempts to deal 2000 damage to "Tree"
        Then the health of "Tree" should be 0
        And the "Tree" should be Destroyed

    Scenario Outline: The damage received by a prop is the same for all character levels
        Given the following characters:
          | Name        | Level   |
          | Brutalitops | <Level> |
        And the following props:
          | Name   | Health |
          | Bridge | 2000   |
        When "Brutalitops" attempts to deal 1000 damage to "Bridge"
        Then the health of "Bridge" should be 1000

        Examples:
          | Level |
          | 1     |
          | 10    |
          | 100   |
          | 1000  |

    Scenario Outline: Characters can only damage props within their range
        Given the following characters:
          | Name        | Class  | X | Y |
          | Lavernica   | Melee  | 1 | 1 |
          | Zippadeedoo | Ranged | 2 | 3 |
        And the following props:
          | Name  | Health | X   | Y   |
          | Tree  | 1000   | 0   | 0   |
          | House | 2000   | -2  | 3   |
          | Chest | 500    | -10 | -20 |
        Then "Lavernica" should be able to damage "Tree"
        But "Lavernica" should not be able to damage "House"
        And "Lavernica" should not be able to damage "Chest"
        And "Zippadeedoo" should be able to damage "Tree"
        And "Zippadeedoo" should be able to damage "House"
        But "Zippadeedoo" should not be able to damage "Chest"