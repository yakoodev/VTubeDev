# TASKS_INDEX

This file is maintained by the agent. It documents the selection rule and avoids hard listings so task names can change freely.

## Selection Rule
- Iterate stage directories in alphabetical order: A, B, C, ...
- Within a stage, iterate substage directories in ascending numeric order (A1, A2, ...).
- Pick the lowest available task index within that substage (e.g., A1_1, then A1_2).
- If a substage has no task with index 1, pick the smallest existing index.
- If a stage has no tasks, move to the next stage.

## Index
- This index is intentionally dynamic. Do not hardcode task names here.
- When selecting a task, scan `tasks/todo/` at runtime using the Selection Rule above.
