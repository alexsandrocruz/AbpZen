import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProposalBlockInstancesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  proposalId?: string;
  }

export function useProposalBlockInstances(input: GetProposalBlockInstancesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["proposalBlockInstances", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-block-instance", {
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

export function useAllProposalBlockInstances() {
  return useQuery({
    queryKey: ["proposalBlockInstances", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-block-instance", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProposalBlockInstance(id: string) {
  return useQuery({
    queryKey: ["proposalBlockInstance", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/proposal-block-instance/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProposalBlockInstance() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/proposal-block-instance", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalBlockInstances"] });
    },
  });
}

export function useUpdateProposalBlockInstance() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/proposal-block-instance/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["proposalBlockInstances"] });
      queryClient.invalidateQueries({ queryKey: ["proposalBlockInstance", data.id] });
    },
  });
}

export function useDeleteProposalBlockInstance() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/proposal-block-instance/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalBlockInstances"] });
    },
  });
}


