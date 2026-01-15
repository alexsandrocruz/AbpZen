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
  import { useCreateProject, useUpdateProject } from "@/lib/abp/hooks/useProjects";
import { toast } from "sonner";

const formSchema = z.object({
  
    title: z.any(),
  
    description: z.any(),
  
    status: z.any(),
  
    budget: z.any(),
  
    dueDate: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ProjectFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ProjectForm({
  isOpen,
  onClose,
  initialValues,
}: ProjectFormProps) {
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

  const createMutation = useCreateProject();
const updateMutation = useUpdateProject();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Project updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Project created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save project:", error);
    toast.error(error.message || "Failed to save project");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Project": "Create Project" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the project." : "Fill in the details to create a new project." }
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
  <Label htmlFor="status" > Status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="budget" > Budget</Label>

<Input id="budget" type = "number" step = "any" {...register("budget") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dueDate" > DueDate</Label>

<Input id="dueDate" type = "date" {...register("dueDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId</Label>

<Input id="clientId" {...register("clientId") } />

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
