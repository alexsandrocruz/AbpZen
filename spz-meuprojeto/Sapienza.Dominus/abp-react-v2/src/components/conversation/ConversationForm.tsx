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
  import { useCreateConversation, useUpdateConversation } from "@/lib/abp/hooks/useConversations";
import { toast } from "sonner";

const formSchema = z.object({
  
    type: z.any(),
  
    name: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ConversationFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ConversationForm({
  isOpen,
  onClose,
  initialValues,
}: ConversationFormProps) {
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

  const createMutation = useCreateConversation();
const updateMutation = useUpdateConversation();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Conversation updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Conversation created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save conversation:", error);
    toast.error(error.message || "Failed to save conversation");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Conversation": "Create Conversation" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the conversation." : "Fill in the details to create a new conversation." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="type" > Type</Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="name" > Name</Label>

<Input id="name" {...register("name") } />

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
