import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateTask, useUpdateTask } from "@/lib/abp/hooks/useTasks";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    description: z.any(),
  
    isCompleted: z.any(),
  
    dueDate: z.any(),
  
    projectId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface TaskFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function TaskForm({
  isOpen,
  onClose,
  initialValues,
}: TaskFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateTask();
const updateMutation = useUpdateTask();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Task updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Task created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save task:", error);
    toast.error(error.message || "Failed to save task");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Task": "Create Task" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the task." : "Fill in the details to create a new task." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="title" > Title * </Label>

<Input id="title" {...register("title") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="description" > Description</Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="isCompleted" > IsCompleted</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="isCompleted"
checked = { watch("isCompleted") }
onCheckedChange = {(checked) => setValue("isCompleted", !!checked)}
              />
  < label htmlFor = "isCompleted" className = "text-sm font-normal" >
    { watch("isCompleted") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="dueDate" > DueDate</Label>

<Input id="dueDate" type = "date" {...register("dueDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="projectId" > ProjectId</Label>

<Input id="projectId" {...register("projectId") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
