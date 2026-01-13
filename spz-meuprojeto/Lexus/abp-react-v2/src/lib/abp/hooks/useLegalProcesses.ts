import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLegalProcessesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  lawyerId?: string;
  clientId?: string;
  }

export function useLegalProcesses(input: GetLegalProcessesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["legalProcesses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/legal-process", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLegalProcesses() {
  return useQuery({
    queryKey: ["legalProcesses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/legal-process", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLegalProcess(id: string) {
  return useQuery({
    queryKey: ["legalProcess", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/legal-process/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/legal-process", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}

export function useUpdateLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/legal-process/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
      queryClient.invalidateQueries({ queryKey: ["legalProcess", data.id] });
    },
  });
}

export function useDeleteLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/legal-process/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: Lawyer <-> Client
 * Given a Lawyer, toggle a Client
 */
export function useToggleClient(lawyerId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLegalProcess();
  const deleteMutation = useDeleteLegalProcess();
  const { data: existing } = useLegalProcesses({ lawyerId: lawyerId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ clientId, isChecked }: { clientId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.clientId === clientId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          lawyerId: lawyerId,
          clientId: clientId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: Lawyer <-> Client
 * Given a Client, toggle a Lawyer
 */
export function useToggleLawyer(clientId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLegalProcess();
  const deleteMutation = useDeleteLegalProcess();
  const { data: existing } = useLegalProcesses({ clientId: clientId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ lawyerId, isChecked }: { lawyerId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.lawyerId === lawyerId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          lawyerId: lawyerId,
          clientId: clientId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}

