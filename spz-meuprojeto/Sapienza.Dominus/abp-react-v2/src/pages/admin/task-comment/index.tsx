import { Shell } from "@/components/layout/shell";
import { TaskCommentList } from "@/components/task-comment/TaskCommentList";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useLocation } from "wouter";

export default function TaskCommentsPage() {
  const [, setLocation] = useLocation();

  return (
    <Shell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="bg-primary/10 p-2 rounded-lg">
              <Box className="h-6 w-6 text-primary" />
            </div>
            <div>
              <h1 className="text-3xl font-bold tracking-tight">TaskComments</h1>
              <p className="text-muted-foreground">Manage your taskcomments</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/task-comment/create")}>
            <Plus className="size-4" />
            New TaskComment
          </Button>
        </div>

        <TaskCommentList onEdit={(item) => setLocation(`/admin/task-comment/${item.id}/edit`)} />
      </div>
    </Shell>
  );
}
