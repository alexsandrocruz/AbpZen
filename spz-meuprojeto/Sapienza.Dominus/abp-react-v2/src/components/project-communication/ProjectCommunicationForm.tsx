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
  import { useCreateProjectCommunication, useUpdateProjectCommunication } from "@/lib/abp/hooks/useProjectCommunications";
import { toast } from "sonner";

const formSchema = z.object({
  
    channel: z.any(),
  
    subject: z.any(),
  
    content: z.any(),
  
    projectId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ProjectCommunicationFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ProjectCommunicationForm({
  isOpen,
  onClose,
  initialValues,
}: ProjectCommunicationFormProps) {
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

  const createMutation = useCreateProjectCommunication();
const updateMutation = useUpdateProjectCommunication();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("ProjectCommunication updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("ProjectCommunication created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save projectcommunication:", error);
    toast.error(error.message || "Failed to save projectcommunication");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit ProjectCommunication": "Create ProjectCommunication" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the projectcommunication." : "Fill in the details to create a new projectcommunication." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="channel" > Channel * </Label>

<Input id="channel" {...register("channel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="subject" > Subject</Label>

<Input id="subject" {...register("subject") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="content" > Content</Label>

<Input id="content" {...register("content") } />

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
